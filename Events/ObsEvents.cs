using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamerBot.Events.OBS
{
    // -----------------------------------------
    // Scene Changed
    // -----------------------------------------
    public class ObsSceneChangedEvent : StreamerBotEventBase
    {
        public string SceneName { get; set; }
        public string PreviousSceneName { get; set; }
    }

    // -----------------------------------------
    // Scene Item Visibility Changed
    // -----------------------------------------
    public class ObsSceneItemVisibilityChangedEvent : StreamerBotEventBase
    {
        public string SceneName { get; set; }
        public string SourceName { get; set; }
        public bool Visible { get; set; }
    }

    // -----------------------------------------
    // Scene Item Transform Changed
    // -----------------------------------------
    public class ObsSceneItemTransformChangedEvent : StreamerBotEventBase
    {
        public string SceneName { get; set; }
        public string SourceName { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public float ScaleX { get; set; }
        public float ScaleY { get; set; }

        public float Rotation { get; set; }
    }

    // -----------------------------------------
    // Transition Begin
    // -----------------------------------------
    public class ObsTransitionBeginEvent : StreamerBotEventBase
    {
        public string TransitionName { get; set; }
        public string FromScene { get; set; }
        public string ToScene { get; set; }
    }

    // -----------------------------------------
    // Transition End
    // -----------------------------------------
    public class ObsTransitionEndEvent : StreamerBotEventBase
    {
        public string TransitionName { get; set; }
        public string FromScene { get; set; }
        public string ToScene { get; set; }
    }

    // -----------------------------------------
    // Streaming Started
    // -----------------------------------------
    public class ObsStreamStartedEvent : StreamerBotEventBase
    {
        public DateTime StartedAt { get; set; }
    }

    // -----------------------------------------
    // Streaming Stopped
    // -----------------------------------------
    public class ObsStreamStoppedEvent : StreamerBotEventBase
    {
        public DateTime StoppedAt { get; set; }
    }

    // -----------------------------------------
    // Recording Started
    // -----------------------------------------
    public class ObsRecordingStartedEvent : StreamerBotEventBase
    {
        public DateTime StartedAt { get; set; }
    }

    // -----------------------------------------
    // Recording Stopped
    // -----------------------------------------
    public class ObsRecordingStoppedEvent : StreamerBotEventBase
    {
        public DateTime StoppedAt { get; set; }
        public string FilePath { get; set; }
    }

    // -----------------------------------------
    // Virtual Camera Started
    // -----------------------------------------
    public class ObsVirtualCameraStartedEvent : StreamerBotEventBase
    {
        public DateTime StartedAt { get; set; }
    }

    // -----------------------------------------
    // Virtual Camera Stopped
    // -----------------------------------------
    public class ObsVirtualCameraStoppedEvent : StreamerBotEventBase
    {
        public DateTime StoppedAt { get; set; }
    }
}
