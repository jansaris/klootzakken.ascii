using Klootzakken.Api.Api;
using Klootzakken.Api.Model;

namespace Klootzakken.Ascii
{
    public class StateMachine : IStateMachine
    {
        private IScreen _screen;
        private readonly GameApi _gameApi;
        private States _state;

        public StateMachine(IScreen screen, GameApi gameApi)
        {
            _screen = screen;
            _gameApi = gameApi;
        }

        public enum States
        {
            Login,
            Lobby,
            InGame
        }

        public string ActiveGame { get; private set; }

        public States State
        {
            get => _state;
            private set
            {
                _state = value;
                _screen.Log($"Switched state to {_state}");
            }
        }

        public void LoggedIn(string token)
        {
            _gameApi.Configuration.AddDefaultHeader("Authorization", $"bearer {token}");
            //_gameApi.Configuration.AccessToken = token;
            State = States.Lobby;
        }

        public void JoinedGame(string game)
        {
            ActiveGame = game;
            State = States.InGame;
        }

        public void ExitGame()
        {
            State = States.Lobby;
        }
    }
}
