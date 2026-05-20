using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StreamerBot.Requests
{
    // -------------------------
    // GetScenes
    // -------------------------
    public class ObsGetScenesRequest : StreamerBotRequest
    {
        public override string Request => "ObsGetScenes";
    }

    public class ObsGetScenesResponse : StreamerBotResponse
    {
        [JsonPropertyName("scenes")]
        public Dictionary<string, object>[] Scenes { get; set; }
    }

    // -------------------------
    // GetSceneItems
    // -------------------------
    public class ObsGetSceneItemsRequest : StreamerBotRequest
    {
        public override string Request => "ObsGetSceneItems";

        [JsonPropertyName("scene")]
        public string Scene { get; set; }
    }

    public class ObsGetSceneItemsResponse : StreamerBotResponse
    {
        [JsonPropertyName("items")]
        public Dictionary<string, object>[] Items { get; set; }
    }

    // -------------------------
    // SetScene
    // -------------------------
    public class ObsSetSceneRequest : StreamerBotRequest
    {
        public override string Request => "ObsSetScene";

        [JsonPropertyName("scene")]
        public string Scene { get; set; }
    }

    public class ObsSetSceneResponse : StreamerBotResponse
    {
    }

    // -------------------------
    // SetSourceVisibility
    // -------------------------
    public class ObsSetSourceVisibilityRequest : StreamerBotRequest
    {
        public override string Request => "ObsSetSourceVisibility";

        [JsonPropertyName("scene")]
        public string Scene { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }
    }

    public class ObsSetSourceVisibilityResponse : StreamerBotResponse
    {
    }

    // -------------------------
    // TriggerHotkey
    // -------------------------
    public class ObsTriggerHotkeyRequest : StreamerBotRequest
    {
        public override string Request => "ObsTriggerHotkey";

        [JsonPropertyName("hotkey")]
        public string Hotkey { get; set; }
    }

    public class ObsTriggerHotkeyResponse : StreamerBotResponse
    {
    }
}
