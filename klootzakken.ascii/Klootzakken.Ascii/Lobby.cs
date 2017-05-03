using System.Collections.Generic;
using Klootzakken.Api.Api;
using Klootzakken.Api.Client;
using Klootzakken.Api.Model;

namespace Klootzakken.Ascii
{
    public class Lobby : ILobby
    {
        private GameApi _client;
        private readonly IStateMachine _stateMachine;
        private readonly IScreen _screen;

        private LobbyView _view = null;

        public Lobby(GameApi client, IStateMachine stateMachine, IScreen screen)
        {
            _client = client;
            _stateMachine = stateMachine;
            _screen = screen;
        }

        public void Do()
        {
            if (_view == null) JoinLobby();
            else ShowLobby();
            
        }

        private void ShowLobby()
        {
            _screen.Display($"Welcome in lobby: {_view.Name}");
            _screen.Display($"There are {_view.AllUsers.Count} gamers in the lobby");
            _screen.Display("We say hello to: ");
            _view.AllUsers.ForEach(g =>
            {
                _screen.Display(g.Name);
            });
            _screen.Display("Type 'Start' to start the game");
            var value = _screen.GetValue();
            if (value.Trim().ToLower() == "start")
            {
                try
                {
                    _client.GetGame(_view.Id);
                }
                catch (ApiException)
                {
                    //The game bestaat blijkbaar niet,
                    //Dan starten wij hem maar, in de catch ^_^
                    _client.StartGame(_view.Id);
                }
                _stateMachine.JoinedGame(_view.Id);
            }
        }

        private void JoinLobby()
        {
            _screen.Display("Available lobbies:");
            var lobbies = _client.ListLobbies();
            var count = 0;
            lobbies.ForEach(l =>
            {
                _screen.Display($"{count++}: {l.Name}");
            });
            _screen.Display("Enter the number to join a game, or type a name to create one:");

            try
            {
                var lobby = CreateOrGetLobby(lobbies);
                _view = _client.JoinLobby(lobby.Id);
            }
            catch (AsciiException ex)
            {
                _screen.Display(ex.Message);
            }
        }

        private LobbyView CreateOrGetLobby(List<LobbyView> lobbies)
        {
            var lobby = _screen.GetValue();
            if (int.TryParse(lobby, out int lobbyId))
            {
                if (lobbies.Count < lobbyId || lobbyId < 0) throw  new AsciiException("Dich vettig menke!");
                return lobbies[lobbyId];
            }

            return _client.CreateLobby(lobby, true);
        }
    }
}