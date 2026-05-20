using System.Collections.Generic;
using System.Threading.Tasks;
using StreamerBot.Requests;

namespace StreamerBot
{
    public class EventsClient
    {
        private readonly StreamerBotConnector _connector;

        public EventsClient(StreamerBotConnector connector)
        {
            _connector = connector;
        }

        // Get all event categories + event names
        public Task<GetEventsResponse> GetAll()
        {
            var req = new GetEventsRequest();
            return _connector.SendRequestAsync<GetEventsRequest, GetEventsResponse>(req);
        }

        // Subscribe to user-selected events
        public Task<SubscribeResponse> Subscribe(Dictionary<string, string[]> events)
        {
            var req = new SubscribeRequest
            {
                Events = events
            };

            return _connector.SendRequestAsync<SubscribeRequest, SubscribeResponse>(req);
        }

        // Get all commands
        public Task<GetCommandsResponse> GetCommands()
        {
            var req = new GetCommandsRequest();
            return _connector.SendRequestAsync<GetCommandsRequest, GetCommandsResponse>(req);
        }
    }
}
