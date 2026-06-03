using System;
using StreamerBot.Events.Kick;

namespace StreamerBot
{
    public class KickClient
    {
        public event Action<KickChatMessageEvent> OnChatMessage;
        public event Action<KickFollowEvent> OnFollow;
        public event Action<KickSubscriptionEvent> OnSubscription;
        public event Action<KickGiftSubEvent> OnGiftSub;
        public event Action<KickRaidEvent> OnRaid;
        public event Action<KickStreamOnlineEvent> OnStreamOnline;
        public event Action<KickStreamOfflineEvent> OnStreamOffline;

        public KickClient(StreamerBotConnector connector)
        {
            connector.RegisterTypedEventHandler<KickChatMessageEvent>("Kick", "ChatMessage", evt => OnChatMessage?.Invoke(evt));
            connector.RegisterTypedEventHandler<KickFollowEvent>("Kick", "Follow", evt => OnFollow?.Invoke(evt));
            connector.RegisterTypedEventHandler<KickSubscriptionEvent>("Kick", "Subscription", evt => OnSubscription?.Invoke(evt));
            connector.RegisterTypedEventHandler<KickGiftSubEvent>("Kick", "GiftSubscription", evt => OnGiftSub?.Invoke(evt));
            connector.RegisterTypedEventHandler<KickRaidEvent>("Kick", "Raid", evt => OnRaid?.Invoke(evt));
            connector.RegisterTypedEventHandler<KickStreamOnlineEvent>("Kick", "StreamOnline", evt => OnStreamOnline?.Invoke(evt));
            connector.RegisterTypedEventHandler<KickStreamOfflineEvent>("Kick", "StreamOffline", evt => OnStreamOffline?.Invoke(evt));
        }
    }
}
