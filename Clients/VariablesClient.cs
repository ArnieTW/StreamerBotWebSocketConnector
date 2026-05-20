using System.Threading.Tasks;
using StreamerBot.Requests;

namespace StreamerBot
{
    public class VariablesClient
    {
        private readonly StreamerBotConnector _connector;

        public VariablesClient(StreamerBotConnector connector)
        {
            _connector = connector;
        }

        public Task<GetGlobalVariablesResponse> GetAll()
        {
            var req = new GetGlobalVariablesRequest();
            return _connector.SendRequestAsync<GetGlobalVariablesRequest, GetGlobalVariablesResponse>(req);
        }

        public Task<SetGlobalVariableResponse> Set(string name, object value)
        {
            var req = new SetGlobalVariableRequest
            {
                Name = name,
                Value = value
            };

            return _connector.SendRequestAsync<SetGlobalVariableRequest, SetGlobalVariableResponse>(req);
        }
    }
}
