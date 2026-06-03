using System;
using System.Threading.Tasks;
using StreamerBot.Events.OBS;
using StreamerBot.Requests;

namespace StreamerBot
{
    public class ObsClient
    {
        private readonly StreamerBotConnector _connector;

        public event Action<ObsSceneChangedEvent> OnSceneChanged;
        public event Action<ObsSceneItemVisibilityChangedEvent> OnSceneItemVisibilityChanged;
        public event Action<ObsTransitionBeginEvent> OnTransitionBegin;
        public event Action<ObsTransitionEndEvent> OnTransitionEnd;

        public ObsClient(StreamerBotConnector connector)
        {
            _connector = connector;

            connector.RegisterTypedEventHandler<ObsSceneChangedEvent>("Obs", "SceneChanged", evt => OnSceneChanged?.Invoke(evt));
            connector.RegisterTypedEventHandler<ObsSceneItemVisibilityChangedEvent>("Obs", "SceneItemVisibilityChanged", evt => OnSceneItemVisibilityChanged?.Invoke(evt));
            connector.RegisterTypedEventHandler<ObsTransitionBeginEvent>("Obs", "TransitionBegin", evt => OnTransitionBegin?.Invoke(evt));
            connector.RegisterTypedEventHandler<ObsTransitionEndEvent>("Obs", "TransitionEnd", evt => OnTransitionEnd?.Invoke(evt));
        }

        public Task<ObsGetScenesResponse> GetScenes()
        {
            var req = new ObsGetScenesRequest();
            return _connector.SendRequestAsync<ObsGetScenesRequest, ObsGetScenesResponse>(req);
        }

        public Task<ObsSetSceneResponse> SetScene(string scene)
        {
            var req = new ObsSetSceneRequest { Scene = scene };
            return _connector.SendRequestAsync<ObsSetSceneRequest, ObsSetSceneResponse>(req);
        }
    }
}
