using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamerBot.Events.YouTube
{
    // ----------------------------------------------------
    // YouTube Chat Message
    // ----------------------------------------------------
    public class YouTubeChatMessageEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public string Message { get; set; }
        public string MessageId { get; set; }

        public bool IsModerator { get; set; }
        public bool IsMember { get; set; }
        public bool IsOwner { get; set; }

        public string ProfileImageUrl { get; set; }
        public string ChannelId { get; set; }
    }

    public class YouTubeFirstWordsEvent : StreamerBotEventBase
    {
    }

    // ----------------------------------------------------
    // YouTube Member Join (Membership Purchase)
    // ----------------------------------------------------
    public class YouTubeMemberJoinEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public string MembershipLevel { get; set; }
        public DateTime JoinedAt { get; set; }
    }

    // ----------------------------------------------------
    // YouTube SuperChat
    // ----------------------------------------------------
    public class YouTubeSuperChatEvent : StreamerBotEventBase
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }

        public string Message { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public string Tier { get; set; }
        public string Color { get; set; }
    }
}
