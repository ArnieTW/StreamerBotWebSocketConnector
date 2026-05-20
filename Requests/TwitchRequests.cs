using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    // -------------------------
    // GetUsers
    // -------------------------
    public class TwitchGetUsersRequest : StreamerBotRequest
    {
        public override string Request => "TwitchGetUsers";

        [JsonPropertyName("users")]
        public string[] Users { get; set; }
    }

    public class TwitchGetUsersResponse : StreamerBotResponse
    {
        [JsonPropertyName("users")]
        public Dictionary<string, object> Users { get; set; }
    }

    // -------------------------
    // GetChannelInfo
    // -------------------------
    public class TwitchGetChannelInfoRequest : StreamerBotRequest
    {
        public override string Request => "TwitchGetChannelInfo";
    }

    public class TwitchGetChannelInfoResponse : StreamerBotResponse
    {
        [JsonPropertyName("channel")]
        public Dictionary<string, object> Channel { get; set; }
    }

    // -------------------------
    // GetModerators
    // -------------------------
    public class TwitchGetModeratorsRequest : StreamerBotRequest
    {
        public override string Request => "TwitchGetModerators";
    }

    public class TwitchGetModeratorsResponse : StreamerBotResponse
    {
        [JsonPropertyName("moderators")]
        public string[] Moderators { get; set; }
    }

    // -------------------------
    // GetSubscribers
    // -------------------------
    public class TwitchGetSubscribersRequest : StreamerBotRequest
    {
        public override string Request => "TwitchGetSubscribers";
    }

    public class TwitchGetSubscribersResponse : StreamerBotResponse
    {
        [JsonPropertyName("subscribers")]
        public Dictionary<string, object>[] Subscribers { get; set; }
    }

    // -------------------------
    // GetRedemptions
    // -------------------------
    public class TwitchGetRedemptionsRequest : StreamerBotRequest
    {
        public override string Request => "TwitchGetRedemptions";
    }

    public class TwitchGetRedemptionsResponse : StreamerBotResponse
    {
        [JsonPropertyName("redemptions")]
        public Dictionary<string, object>[] Redemptions { get; set; }
    }

    // -------------------------
    // GetPredictions
    // -------------------------
    public class TwitchGetPredictionsRequest : StreamerBotRequest
    {
        public override string Request => "TwitchGetPredictions";
    }

    public class TwitchGetPredictionsResponse : StreamerBotResponse
    {
        [JsonPropertyName("predictions")]
        public Dictionary<string, object>[] Predictions { get; set; }
    }

    // -------------------------
    // GetPolls
    // -------------------------
    public class TwitchGetPollsRequest : StreamerBotRequest
    {
        public override string Request => "TwitchGetPolls";
    }

    public class TwitchGetPollsResponse : StreamerBotResponse
    {
        [JsonPropertyName("polls")]
        public Dictionary<string, object>[] Polls { get; set; }
    }
}
