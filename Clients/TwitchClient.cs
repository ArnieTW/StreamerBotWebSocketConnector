using System;
using System.Threading.Tasks;
using StreamerBot.Events.Twitch;
using StreamerBot.Requests;

namespace StreamerBot
{
    public class TwitchClient
    {
        private readonly StreamerBotConnector _connector;

        public event Action<TwitchChatMessageEvent> OnChatMessage;
        public event Action<TwitchFollowEvent> OnFollow;
        public event Action<TwitchRaidEvent> OnRaid;
        public event Action<TwitchCheerEvent> OnCheer;
        public event Action<TwitchSubscriptionEvent> OnSubscription;
        public event Action<TwitchGiftSubEvent> OnGiftSub;
        public event Action<TwitchRedemptionEvent> OnRedemption;
        public event Action<TwitchWhisperEvent> OnWhisper;

        public TwitchClient(StreamerBotConnector connector)
        {
            _connector = connector;

            connector.RegisterTypedEventHandler<TwitchChatMessageEvent>("Twitch", "ChatMessage", evt => OnChatMessage?.Invoke(evt));
            connector.RegisterTypedEventHandler<TwitchFollowEvent>("Twitch", "Follow", evt => OnFollow?.Invoke(evt));
            connector.RegisterTypedEventHandler<TwitchRaidEvent>("Twitch", "Raid", evt => OnRaid?.Invoke(evt));
            connector.RegisterTypedEventHandler<TwitchCheerEvent>("Twitch", "Cheer", evt => OnCheer?.Invoke(evt));
            connector.RegisterTypedEventHandler<TwitchSubscriptionEvent>("Twitch", "Sub", evt => OnSubscription?.Invoke(evt));
            connector.RegisterTypedEventHandler<TwitchGiftSubEvent>("Twitch", "GiftSub", evt => OnGiftSub?.Invoke(evt));
            connector.RegisterTypedEventHandler<TwitchRedemptionEvent>("Twitch", "RewardRedemption", evt => OnRedemption?.Invoke(evt));
            connector.RegisterTypedEventHandler<TwitchWhisperEvent>("Twitch", "Whisper", evt => OnWhisper?.Invoke(evt));
        }

        // Example request helper
        public Task<TwitchGetUsersResponse> GetUsers(params string[] users)
        {
            var req = new TwitchGetUsersRequest
            {
                Users = users
            };

            return _connector.SendRequestAsync<TwitchGetUsersRequest, TwitchGetUsersResponse>(req);
        }
    }
}
