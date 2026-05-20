using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    // -------------------------
    // GetActions
    // -------------------------
    public class GetActionsRequest : StreamerBotRequest
    {
        public override string Request => "GetActions";
    }

    public class GetActionsResponse : StreamerBotResponse
    {
        [JsonPropertyName("actions")]
        public Dictionary<string, string> Actions { get; set; }
    }

    // -------------------------
    // GetActionInfo
    // -------------------------
    public class GetActionInfoRequest : StreamerBotRequest
    {
        public override string Request => "GetActionInfo";

        [JsonPropertyName("action")]
        public string ActionId { get; set; }
    }

    public class GetActionInfoResponse : StreamerBotResponse
    {
        [JsonPropertyName("action")]
        public Dictionary<string, object> Action { get; set; }
    }

    // -------------------------
    // DoAction
    // -------------------------
    public class DoActionRequest : StreamerBotRequest
    {
        public override string Request => "DoAction";

        [JsonPropertyName("action")]
        public ActionRef Action { get; set; }

        [JsonPropertyName("args")]
        public Dictionary<string, object> Args { get; set; } = new();
    }

    public class DoActionResponse : StreamerBotResponse
    {
    }

    public class ActionRef
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
