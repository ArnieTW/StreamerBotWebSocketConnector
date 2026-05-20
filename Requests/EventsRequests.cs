using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    // -------------------------
    // GetEvents
    // -------------------------
    public class GetEventsRequest : StreamerBotRequest
    {
        public override string Request => "GetEvents";
    }

    public class GetEventsResponse : StreamerBotResponse
    {
        [JsonPropertyName("events")]
        public Dictionary<string, string[]> Events { get; set; } = new();
    }

    // -------------------------
    // Subscribe
    // -------------------------
    public class SubscribeRequest : StreamerBotRequest
    {
        public override string Request => "Subscribe";

        [JsonPropertyName("events")]
        public Dictionary<string, string[]> Events { get; set; } = new();
    }

    public class SubscribeResponse : StreamerBotResponse
    {
        [JsonPropertyName("events")]
        public Dictionary<string, string[]> Events { get; set; } = new();
    }
}
