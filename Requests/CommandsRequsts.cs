using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    public class GetCommandsRequest : StreamerBotRequest
    {
        public override string Request => "GetCommands";
    }

    public class GetCommandsResponse : StreamerBotResponse
    {
        [JsonPropertyName("commands")]
        public Dictionary<string, object> Commands { get; set; }
    }
}
