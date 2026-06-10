using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    // -------------------------
    // GetActiveViewers
    // -------------------------
    public class GetActiveViewersRequest : StreamerBotRequest
    {
        public override string Request => "GetActiveViewers";
    }

    public class GetActiveViewersResponse : StreamerBotResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("viewers")]
        public ActiveViewer[] Viewers { get; set; } = [];
    }

    public class ActiveViewer
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [JsonPropertyName("display")]
        public string? Display { get; set; }

        [JsonPropertyName("subscribed")]
        public bool Subscribed { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("groups")]
        public string[] Groups { get; set; } = [];

        [JsonPropertyName("channelPointsUsed")]
        public int ChannelPointsUsed { get; set; }

        [JsonPropertyName("previousActive")]
        public string? PreviousActive { get; set; }

        [JsonPropertyName("exempt")]
        public bool Exempt { get; set; }
    }

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

    // -------------------------
    // UnSubscribe
    // -------------------------
    public class UnSubscribeRequest : StreamerBotRequest
    {
        public override string Request => "UnSubscribe";

        [JsonPropertyName("events")]
        public Dictionary<string, string[]> Events { get; set; } = new();
    }

    public class UnSubscribeResponse : StreamerBotResponse
    {
        [JsonPropertyName("events")]
        public Dictionary<string, string[]> Events { get; set; } = new();
    }
}
