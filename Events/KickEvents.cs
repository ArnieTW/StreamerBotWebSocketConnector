using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamerBot.Events.Kick
{
    // ----------------------------------------------------
    // Kick Chat Message
    // ----------------------------------------------------
    public class KickChatMessageEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public string Message { get; set; }
        public string MessageId { get; set; }

        public bool IsModerator { get; set; }
        public bool IsSubscriber { get; set; }
        public bool IsVip { get; set; }
        public bool IsStreamer { get; set; }

        public string Color { get; set; }
        public KickBadge[] Badges { get; set; }
    }

    public class KickFirstWordsEvent : StreamerBotEventBase
    {
    }

    public class KickBadge
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    // ----------------------------------------------------
    // Kick Follow
    // ----------------------------------------------------
    public class KickFollowEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public DateTime FollowedAt { get; set; }
    }

    // ----------------------------------------------------
    // Kick Subscription
    // ----------------------------------------------------
    public class KickSubscriptionEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public string Tier { get; set; }
        public int Months { get; set; }
        public bool IsGift { get; set; }
        public string Message { get; set; }
    }

    // ----------------------------------------------------
    // Kick Gifted Subscriptions
    // ----------------------------------------------------
    public class KickGiftSubEvent : StreamerBotEventBase
    {
        public string GifterUserName { get; set; }
        public string GifterDisplayName { get; set; }

        public int Count { get; set; }
        public string Tier { get; set; }
    }

    // ----------------------------------------------------
    // Kick Raid
    // ----------------------------------------------------
    public class KickRaidEvent : StreamerBotEventBase
    {
        public string FromUser { get; set; }
        public string FromDisplayName { get; set; }

        public int ViewerCount { get; set; }
    }

    // ----------------------------------------------------
    // Kick Stream Online
    // ----------------------------------------------------
    public class KickStreamOnlineEvent : StreamerBotEventBase
    {
        public DateTime StartedAt { get; set; }
        public string StreamTitle { get; set; }
        public string Category { get; set; }
    }

    // ----------------------------------------------------
    // Kick Stream Offline
    // ----------------------------------------------------
    public class KickStreamOfflineEvent : StreamerBotEventBase
    {
        public DateTime StoppedAt { get; set; }
    }
}
