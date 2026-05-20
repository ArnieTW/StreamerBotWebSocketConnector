using System;
using System.Text.Json.Serialization;

namespace StreamerBot
{
    public abstract class StreamerBotRequest
    {
        [JsonPropertyName("request")]
        public abstract string Request { get; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
