using System.Threading.Tasks;

namespace StreamerBot
{
    public class StreamerBotClient
    {
        private readonly StreamerBotConnector _connector;

        public TwitchClient Twitch { get; }
        public KickClient Kick { get; }
        public YouTubeClient YouTube { get; }
        public ObsClient OBS { get; }
        public ActionsClient Actions { get; }
        public VariablesClient Variables { get; }
        public EventsClient Events { get; }

        public StreamerBotClient(string wsUrl)
        {
            _connector = new StreamerBotConnector(wsUrl);

            Twitch = new TwitchClient(_connector);
            Kick = new KickClient(_connector);
            YouTube = new YouTubeClient(_connector);
            OBS = new ObsClient(_connector);
            Actions = new ActionsClient(_connector);
            Variables = new VariablesClient(_connector);
            Events = new EventsClient(_connector);
        }

        public Task ConnectAsync() => _connector.ConnectAsync();
    }
}
