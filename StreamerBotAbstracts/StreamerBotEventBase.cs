using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamerBot.Events
{
    public abstract class StreamerBotEventBase
    {
        // Raw JSON payload for future-proofing
        public JsonElement Raw { get; set; }

        // Unknown fields captured automatically
        [JsonExtensionData]
        public Dictionary<string, object> Extra { get; set; } = new();
    }

    public sealed class StreamerBotRawEvent : StreamerBotEventBase
    {
    }
}
