using System;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text.Json;
using System.IO;
using StreamerBot.Events;

namespace StreamerBot
{
    public class StreamerBotConnector : IDisposable
    {
        private static readonly JsonSerializerOptions EventJsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly ClientWebSocket _ws = new();
        private readonly Uri _uri;
        private readonly CancellationTokenSource _cts = new();
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pending = new();
        private readonly TaskCompletionSource<AuthChallenge> _hello = new(TaskCreationOptions.RunContinuationsAsynchronously);

        // Raw message event
        public event Action<string> OnRawMessageReceived;

        // Raw event (category, name, payload)
        public event Action<string, string, JsonElement> OnEventReceived;

        public event Action<string, string, StreamerBotEventBase> OnTypedEventReceived;

        public event Action<string> OnUnrecognizedEventReceived;

        public event Action OnDisconnected;

        // Typed event handlers
        private readonly ConcurrentDictionary<(string Category, string Name), Delegate> _typedHandlers
            = new();

        private readonly ConcurrentDictionary<(string Category, string Name), Type> _typedHandlerEventTypes
            = new();

        public StreamerBotConnector(string wsUrl)
        {
            _uri = new Uri(wsUrl);
        }

        // ------------------------------------------------------------
        // Connection
        // ------------------------------------------------------------
        public async Task ConnectAsync()
        {
            await _ws.ConnectAsync(_uri, _cts.Token);
            _ = Task.Run(ReceiveLoop);
        }

        public Task AuthenticateIfNeededAsync(string? password)
            => AuthenticateIfNeededAsync(password, _cts.Token);

        public async Task AuthenticateIfNeededAsync(string? password, CancellationToken cancellationToken)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken);
            var hello = await _hello.Task.WaitAsync(TimeSpan.FromSeconds(10), linked.Token);
            if (!hello.RequiresAuthentication)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Streamer.bot requires a websocket password.");
            }

            var authentication = CreateAuthenticationToken(password, hello.Salt, hello.Challenge);
            var id = Guid.NewGuid().ToString();
            var request = JsonSerializer.Serialize(new
            {
                request = "Authenticate",
                id,
                authentication
            });
            var responseJson = await SendRequestAsync(request, id, linked.Token);
            using var response = JsonDocument.Parse(responseJson);
            var status = response.RootElement.TryGetProperty("status", out var statusProperty)
                ? statusProperty.GetString()
                : null;
            if (!string.Equals(status, "ok", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Streamer.bot authentication failed.");
            }
        }

        // ------------------------------------------------------------
        // Typed event handler registration
        // ------------------------------------------------------------
        public void RegisterTypedEventHandler<T>(string category, string eventName, StreamerBotEventHandler<T> handler)
            where T : StreamerBotEventBase
        {
            var key = (category.Trim(), eventName.Trim());
            _typedHandlers[key] = handler;
            _typedHandlerEventTypes[key] = typeof(T);
        }

        // ------------------------------------------------------------
        // Raw event handler registration (JsonElement)
        // ------------------------------------------------------------
        public void RegisterEventHandler(string category, string eventName, Action<JsonElement> handler)
        {
            var key = (category.Trim(), eventName.Trim());
            _typedHandlers[key] = handler;
            _typedHandlerEventTypes.TryRemove(key, out _);
        }

        // ------------------------------------------------------------
        // Send raw JSON request
        // ------------------------------------------------------------
        public Task<string> SendRequestAsync(string requestJson, string id)
            => SendRequestAsync(requestJson, id, _cts.Token);

        public async Task<string> SendRequestAsync(string requestJson, string id, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            _pending[id] = tcs;

            using var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken);
            using var registration = linked.Token.Register(() =>
            {
                if (_pending.TryRemove(id, out var pending))
                {
                    pending.TrySetCanceled(linked.Token);
                }
            });

            try
            {
                var buffer = Encoding.UTF8.GetBytes(requestJson);
                await _ws.SendAsync(buffer, WebSocketMessageType.Text, true, linked.Token);
                return await tcs.Task;
            }
            catch
            {
                _pending.TryRemove(id, out _);
                throw;
            }
        }

        // ------------------------------------------------------------
        // Send typed request
        // ------------------------------------------------------------
        public Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : StreamerBotRequest
            where TResponse : StreamerBotResponse
            => SendRequestAsync<TRequest, TResponse>(request, _cts.Token);

        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : StreamerBotRequest
            where TResponse : StreamerBotResponse
        {
            string id = request.Id ?? Guid.NewGuid().ToString();
            request.Id = id;

            var json = JsonSerializer.Serialize(request);
            var responseJson = await SendRequestAsync(json, id, cancellationToken);

            return JsonSerializer.Deserialize<TResponse>(responseJson);
        }

        // ------------------------------------------------------------
        // Receive Loop
        // ------------------------------------------------------------
        private async Task ReceiveLoop()
        {
            var buffer = new byte[8192];

            while (!_cts.IsCancellationRequested)
            {
                string json;
                try
                {
                    json = await ReceiveMessageAsync(buffer);
                }
                catch
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(json))
                    break;

                OnRawMessageReceived?.Invoke(json);

                JsonDocument doc;
                try
                {
                    doc = JsonDocument.Parse(json);
                }
                catch
                {
                    continue;
                }

                using (doc)
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("request", out var requestProp)
                        && string.Equals(requestProp.GetString(), "Hello", StringComparison.OrdinalIgnoreCase))
                    {
                        _hello.TrySetResult(ReadAuthChallenge(root));
                    }

                    // --------------------------------------------------------
                    // Handle responses (messages with "id")
                    // --------------------------------------------------------
                    if (root.TryGetProperty("id", out var idProp))
                    {
                        var id = idProp.GetString();
                        if (id != null && _pending.TryRemove(id, out var tcs))
                        {
                            tcs.TrySetResult(json);
                            continue;
                        }
                    }

                    // --------------------------------------------------------
                    // Handle events
                    // --------------------------------------------------------
                    if (root.TryGetProperty("event", out var evt))
                    {
                        if (TryReadEventIdentity(evt, out var category, out var eventName))
                        {
                            var payload = root.TryGetProperty("data", out var data)
                                ? data.Clone()
                                : CreateEmptyPayload();

                            var typed = CreateEventObject(category, eventName, payload);
                            OnTypedEventReceived?.Invoke(category, eventName, typed);

                            if (_typedHandlers.TryGetValue((category, eventName), out var del))
                            {
                                if (del is Action<JsonElement> rawHandler)
                                {
                                    rawHandler(payload);
                                }
                                else
                                {
                                    del.DynamicInvoke(typed);
                                }
                            }
                        }
                        else
                        {
                            OnUnrecognizedEventReceived?.Invoke(json);
                        }
                    }
                }
            }

            OnDisconnected?.Invoke();
        }

        private static AuthChallenge ReadAuthChallenge(JsonElement root)
        {
            if (root.TryGetProperty("authentication", out var auth)
                && auth.TryGetProperty("salt", out var salt)
                && auth.TryGetProperty("challenge", out var challenge))
            {
                return new AuthChallenge(true, salt.GetString() ?? string.Empty, challenge.GetString() ?? string.Empty);
            }

            return new AuthChallenge(false, string.Empty, string.Empty);
        }

        private static bool TryReadEventIdentity(JsonElement evt, out string category, out string eventName)
        {
            category = string.Empty;
            eventName = string.Empty;

            if (evt.ValueKind == JsonValueKind.String)
            {
                return TryReadEventKey(evt.GetString(), out category, out eventName);
            }

            if (evt.ValueKind != JsonValueKind.Object)
            {
                return false;
            }

            if (TryReadEventPair(evt, "category", "name", out category, out eventName)
                || TryReadEventPair(evt, "source", "name", out category, out eventName)
                || TryReadEventPair(evt, "source", "type", out category, out eventName)
                || TryReadEventPair(evt, "source", "event", out category, out eventName)
                || TryReadEventPair(evt, "category", "type", out category, out eventName)
                || TryReadEventPair(evt, "category", "event", out category, out eventName))
            {
                return true;
            }

            if (TryReadStringProperty(evt, "name", out eventName)
                || TryReadStringProperty(evt, "type", out eventName)
                || TryReadStringProperty(evt, "event", out eventName))
            {
                category = string.Empty;
                return eventName.Length > 0;
            }

            return false;
        }

        private static JsonElement CreateEmptyPayload()
        {
            using var document = JsonDocument.Parse("{}");
            return document.RootElement.Clone();
        }

        private static bool TryReadEventPair(
            JsonElement evt,
            string categoryPropertyName,
            string eventPropertyName,
            out string category,
            out string eventName)
        {
            category = string.Empty;
            eventName = string.Empty;

            if (!TryReadStringProperty(evt, categoryPropertyName, out category)
                || !TryReadStringProperty(evt, eventPropertyName, out eventName))
            {
                return false;
            }

            if (category.Length == 0 || eventName.Length == 0)
            {
                return false;
            }

            return true;
        }

        private static bool TryReadStringProperty(JsonElement element, string propertyName, out string value)
        {
            value = string.Empty;

            if (!element.TryGetProperty(propertyName, out var property)
                || property.ValueKind != JsonValueKind.String)
            {
                return false;
            }

            value = property.GetString() ?? string.Empty;
            return value.Length > 0;
        }

        private static bool TryReadEventKey(string? key, out string category, out string eventName)
        {
            category = string.Empty;
            eventName = string.Empty;

            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            var separator = key.IndexOf('.');
            if (separator < 0)
            {
                eventName = key.Trim();
                return eventName.Length > 0;
            }

            category = key[..separator].Trim();
            eventName = key[(separator + 1)..].Trim();
            return category.Length > 0 && eventName.Length > 0;
        }

        private StreamerBotEventBase CreateEventObject(string category, string eventName, JsonElement payload)
        {
            var key = (category, eventName);
            if (_typedHandlerEventTypes.TryGetValue(key, out var registeredType)
                && TryCreateSpecificEvent(registeredType, category, eventName, payload, out var registeredEvent))
            {
                return registeredEvent;
            }

            if (StreamerBotEventRegistry.TryGetEventType(category, eventName, out var type)
                && TryCreateSpecificEvent(type, category, eventName, payload, out var catalogEvent))
            {
                return catalogEvent;
            }

            var raw = new StreamerBotRawEvent();
            PopulateEventMetadata(raw, category, eventName, payload);
            return raw;
        }

        private static bool TryCreateSpecificEvent(
            Type type,
            string category,
            string eventName,
            JsonElement payload,
            out StreamerBotEventBase evt)
        {
            evt = null!;
            try
            {
                var typed = (StreamerBotEventBase?)JsonSerializer.Deserialize(payload.GetRawText(), type, EventJsonOptions);
                if (typed is not null)
                {
                    PopulateEventMetadata(typed, category, eventName, payload);
                    evt = typed;
                    return true;
                }
            }
            catch
            {
                // Fall back to a raw event object when a specific payload shape drifts.
            }

            return false;
        }

        private static void PopulateEventMetadata(StreamerBotEventBase evt, string category, string eventName, JsonElement payload)
        {
            evt.EventSource = category;
            evt.EventType = eventName;
            evt.Raw = payload;
        }

        private static string CreateAuthenticationToken(string password, string salt, string challenge)
        {
            var secret = Sha256Base64(password + salt);
            return Sha256Base64(secret + challenge);
        }

        private static string Sha256Base64(string value)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
        }

        private async Task<string> ReceiveMessageAsync(byte[] buffer)
        {
            using var stream = new MemoryStream();
            WebSocketReceiveResult result;
            do
            {
                result = await _ws.ReceiveAsync(buffer, _cts.Token);
                if (result.MessageType == WebSocketMessageType.Close)
                    return string.Empty;

                stream.Write(buffer, 0, result.Count);
            }
            while (!result.EndOfMessage);

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        // ------------------------------------------------------------
        // Dispose
        // ------------------------------------------------------------
        public void Dispose()
        {
            try { _cts.Cancel(); } catch { }
            try { _ws.Dispose(); } catch { }
        }

        private sealed record AuthChallenge(bool RequiresAuthentication, string Salt, string Challenge);
    }
}
