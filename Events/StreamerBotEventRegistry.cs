using System;
using System.Collections.Generic;
using System.Linq;
using StreamerBot.Events.Kick;
using StreamerBot.Events.OBS;
using StreamerBot.Events.Twitch;
using StreamerBot.Events.YouTube;

namespace StreamerBot.Events
{
    public static class StreamerBotEventRegistry
    {
        public static readonly Dictionary<(string Category, string Name), Type> EventTypes =
            new()
            {
                // ----------------------------------------------------
                // Twitch Events
                // ----------------------------------------------------
                { ("Twitch", "ChatMessage"), typeof(TwitchChatMessageEvent) },
                { ("Twitch", "FirstWord"), typeof(TwitchFirstWordEvent) },
                { ("Twitch", "Follow"), typeof(TwitchFollowEvent) },
                { ("Twitch", "Raid"), typeof(TwitchRaidEvent) },
                { ("Twitch", "Cheer"), typeof(TwitchCheerEvent) },
                { ("Twitch", "Subscription"), typeof(TwitchSubscriptionEvent) },
                { ("Twitch", "GiftSub"), typeof(TwitchGiftSubEvent) },
                { ("Twitch", "Redemption"), typeof(TwitchRedemptionEvent) },
                { ("Twitch", "Whisper"), typeof(TwitchWhisperEvent) },
                { ("Twitch", "PredictionStarted"), typeof(TwitchPredictionStartedEvent) },
                { ("Twitch", "PredictionUpdated"), typeof(TwitchPredictionUpdatedEvent) },
                { ("Twitch", "PredictionCompleted"), typeof(TwitchPredictionCompletedEvent) },

                // ----------------------------------------------------
                // OBS Events
                // ----------------------------------------------------
                { ("OBS", "SceneChanged"), typeof(ObsSceneChangedEvent) },
                { ("OBS", "SceneItemVisibilityChanged"), typeof(ObsSceneItemVisibilityChangedEvent) },
                { ("OBS", "SceneItemTransformChanged"), typeof(ObsSceneItemTransformChangedEvent) },
                { ("OBS", "TransitionBegin"), typeof(ObsTransitionBeginEvent) },
                { ("OBS", "TransitionEnd"), typeof(ObsTransitionEndEvent) },
                { ("OBS", "StreamStarted"), typeof(ObsStreamStartedEvent) },
                { ("OBS", "StreamStopped"), typeof(ObsStreamStoppedEvent) },
                { ("OBS", "RecordingStarted"), typeof(ObsRecordingStartedEvent) },
                { ("OBS", "RecordingStopped"), typeof(ObsRecordingStoppedEvent) },
                { ("OBS", "VirtualCameraStarted"), typeof(ObsVirtualCameraStartedEvent) },
                { ("OBS", "VirtualCameraStopped"), typeof(ObsVirtualCameraStoppedEvent) },

                // ----------------------------------------------------
                // YouTube Events
                // ----------------------------------------------------
                { ("YouTube", "ChatMessage"), typeof(YouTubeChatMessageEvent) },
                { ("YouTube", "FirstWords"), typeof(YouTubeFirstWordsEvent) },
                { ("YouTube", "MemberJoin"), typeof(YouTubeMemberJoinEvent) },
                { ("YouTube", "SuperChat"), typeof(YouTubeSuperChatEvent) },

                // ----------------------------------------------------
                // Kick Events
                // ----------------------------------------------------
                { ("Kick", "ChatMessage"), typeof(KickChatMessageEvent) },
                { ("Kick", "FirstWords"), typeof(KickFirstWordsEvent) },
                { ("Kick", "Follow"), typeof(KickFollowEvent) },
                { ("Kick", "Subscription"), typeof(KickSubscriptionEvent) },
                { ("Kick", "GiftSub"), typeof(KickGiftSubEvent) },
                { ("Kick", "Raid"), typeof(KickRaidEvent) },
                { ("Kick", "StreamOnline"), typeof(KickStreamOnlineEvent) },
                { ("Kick", "StreamOffline"), typeof(KickStreamOfflineEvent) },
            };

        public static bool TryGetEventType(string category, string eventName, out Type type)
        {
            if (EventTypes.TryGetValue((category, eventName), out type!))
            {
                return true;
            }

            var match = EventTypes.FirstOrDefault(pair =>
                string.Equals(pair.Key.Category, category, StringComparison.OrdinalIgnoreCase)
                && string.Equals(pair.Key.Name, eventName, StringComparison.OrdinalIgnoreCase));

            if (match.Value is not null)
            {
                type = match.Value;
                return true;
            }

            if (IsCommandEvent(category, eventName))
            {
                type = typeof(StreamerBotCommandEvent);
                return true;
            }

            if (IsActionEvent(category, eventName))
            {
                type = typeof(StreamerBotActionEvent);
                return true;
            }

            type = null!;
            return false;
        }

        private static bool IsCommandEvent(string category, string eventName)
            => string.Equals(category, "Command", StringComparison.OrdinalIgnoreCase)
                && (string.Equals(eventName, "Triggered", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(eventName, "Message", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(eventName, "Whisper", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(eventName, "BotWhisper", StringComparison.OrdinalIgnoreCase)
                    || eventName.Contains("Cooldown", StringComparison.OrdinalIgnoreCase));

        private static bool IsActionEvent(string category, string eventName)
            => (string.Equals(category, "Action", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(category, "Raw", StringComparison.OrdinalIgnoreCase))
                && (string.Equals(eventName, "Action", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(eventName, "ActionCompleted", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(eventName, "SubAction", StringComparison.OrdinalIgnoreCase));
    }

    public sealed class StreamerBotCommandEvent : StreamerBotEventBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public int Counter { get; set; }
        public int UserCounter { get; set; }
        public string Message { get; set; }
        public StreamerBotUser User { get; set; }
    }

    public sealed class StreamerBotActionEvent : StreamerBotEventBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ActionId { get; set; }
    }

    public sealed class StreamerBotUser
    {
        public string Display { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
        public bool Subscribed { get; set; }
        public string Type { get; set; }
    }
}
