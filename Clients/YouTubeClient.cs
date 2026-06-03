using System;
using StreamerBot.Events.YouTube;

namespace StreamerBot
{
    public class YouTubeClient
    {
        public event Action<YouTubeChatMessageEvent> OnChatMessage;
        public event Action<YouTubeMemberJoinEvent> OnMemberJoin;
        public event Action<YouTubeSuperChatEvent> OnSuperChat;

        public YouTubeClient(StreamerBotConnector connector)
        {
            connector.RegisterTypedEventHandler<YouTubeChatMessageEvent>("YouTube", "Message", evt => OnChatMessage?.Invoke(evt));
            connector.RegisterTypedEventHandler<YouTubeMemberJoinEvent>("YouTube", "NewSponsor", evt => OnMemberJoin?.Invoke(evt));
            connector.RegisterTypedEventHandler<YouTubeSuperChatEvent>("YouTube", "SuperChat", evt => OnSuperChat?.Invoke(evt));
        }
    }
}
