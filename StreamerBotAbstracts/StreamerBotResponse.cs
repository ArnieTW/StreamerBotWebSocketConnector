using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamerBot
{
    public abstract class StreamerBotResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        // Catch-all for unknown fields
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extra { get; set; }
    }
}
