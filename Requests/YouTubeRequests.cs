using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    // -------------------------
    // GetChannelInfo
    // -------------------------
    public class YouTubeGetChannelInfoRequest : StreamerBotRequest
    {
        public override string Request => "YouTubeGetChannelInfo";
    }

    public class YouTubeGetChannelInfoResponse : StreamerBotResponse
    {
        [JsonPropertyName("channel")]
        public Dictionary<string, object> Channel { get; set; }
    }

    // -------------------------
    // GetChatMessages
    // -------------------------
    public class YouTubeGetChatMessagesRequest : StreamerBotRequest
    {
        public override string Request => "YouTubeGetChatMessages";
    }

    public class YouTubeGetChatMessagesResponse : StreamerBotResponse
    {
        [JsonPropertyName("messages")]
        public Dictionary<string, object>[] Messages { get; set; }
    }
}
