using System.Collections.Generic;
using System.Threading.Tasks;
using StreamerBot.Requests;

namespace StreamerBot
{
    public class ActionsClient
    {
        private readonly StreamerBotConnector _connector;

        public ActionsClient(StreamerBotConnector connector)
        {
            _connector = connector;
        }

        // Get all actions
        public Task<GetActionsResponse> GetAll()
        {
            var req = new GetActionsRequest();
            return _connector.SendRequestAsync<GetActionsRequest, GetActionsResponse>(req);
        }

        // Get info about a specific action
        public Task<GetActionInfoResponse> GetInfo(string actionId)
        {
            var req = new GetActionInfoRequest
            {
                ActionId = actionId
            };

            return _connector.SendRequestAsync<GetActionInfoRequest, GetActionInfoResponse>(req);
        }

        // Run an action by ID or name
        public Task<DoActionResponse> Run(string actionIdOrName, Dictionary<string, object> args = null)
        {
            var req = new DoActionRequest
            {
                Action = new ActionRef
                {
                    Id = actionIdOrName,
                    Name = actionIdOrName
                },
                Args = args ?? new Dictionary<string, object>()
            };

            return _connector.SendRequestAsync<DoActionRequest, DoActionResponse>(req);
        }
    }
}
