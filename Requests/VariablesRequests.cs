using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    // -------------------------
    // GetGlobalVariables
    // -------------------------
    public class GetGlobalVariablesRequest : StreamerBotRequest
    {
        public override string Request => "GetGlobalVariables";
    }

    public class GetGlobalVariablesResponse : StreamerBotResponse
    {
        [JsonPropertyName("variables")]
        public Dictionary<string, object> Variables { get; set; }
    }

    // -------------------------
    // SetGlobalVariable
    // -------------------------
    public class SetGlobalVariableRequest : StreamerBotRequest
    {
        public override string Request => "SetGlobalVariable";

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }
    }

    public class SetGlobalVariableResponse : StreamerBotResponse
    {
    }
}
