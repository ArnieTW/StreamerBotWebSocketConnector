using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamerBot.Events.Twitch
{
    // Base class already provides:
    // - JsonElement Raw
    // - Dictionary<string, JsonElement> Extra

    public class TwitchChatMessageEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Message { get; set; }

        public bool IsModerator { get; set; }
        public bool IsSubscriber { get; set; }
        public bool IsVip { get; set; }
        public bool IsBroadcaster { get; set; }

        public string Color { get; set; }
        public TwitchBadge[] Badges { get; set; }
    }

    public class TwitchFirstWordEvent : StreamerBotEventBase
    {
    }

    public class TwitchBadge
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class TwitchFollowEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public DateTime FollowedAt { get; set; }
    }

    public class TwitchRaidEvent : StreamerBotEventBase
    {
        public string FromUser { get; set; }
        public string FromDisplayName { get; set; }
        public int ViewerCount { get; set; }
    }

    public class TwitchCheerEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int Bits { get; set; }
        public string Message { get; set; }
    }

    public class TwitchSubscriptionEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public int Months { get; set; }
        public int Streak { get; set; }
        public bool IsGift { get; set; }
        public string Tier { get; set; }
        public string Message { get; set; }
    }

    public class TwitchGiftSubEvent : StreamerBotEventBase
    {
        public string GifterUserName { get; set; }
        public string GifterDisplayName { get; set; }

        public int Count { get; set; }
        public string Tier { get; set; }
    }

    public class TwitchRedemptionEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public string RewardId { get; set; }
        public string RewardName { get; set; }
        public string Input { get; set; }
    }

    public class TwitchWhisperEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Message { get; set; }
    }

    public class TwitchPredictionStartedEvent : StreamerBotEventBase
    {
        public string Title { get; set; }
        public Dictionary<string, object>[] Outcomes { get; set; }
    }

    public class TwitchPredictionUpdatedEvent : StreamerBotEventBase
    {
        public string Title { get; set; }
        public Dictionary<string, object>[] Outcomes { get; set; }
    }

    public class TwitchPredictionCompletedEvent : StreamerBotEventBase
    {
        public string Title { get; set; }
        public string WinningOutcomeId { get; set; }
        public Dictionary<string, object>[] Outcomes { get; set; }
    }
}
