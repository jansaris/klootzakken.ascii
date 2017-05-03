using System.Linq;
using System.Threading.Tasks;
using Klootzakken.Api.Api;
using Klootzakken.Api.Model;

namespace Klootzakken.Ascii
{
    public class Game : IGame
    {
        private readonly IStateMachine _stateMachine;
        private readonly IScreen _screen;
        private readonly GameApi _client;
        private GameView _previousGameView;

        public Game(IScreen screen, IStateMachine stateMachine, GameApi client)
        {
            _screen = screen;
            _stateMachine = stateMachine;
            _client = client;
        }

        public void Do()
        {
            var info = _client.GetGame(_stateMachine.ActiveGame);
            ShowGameState(info);
            if (info.You.PossibleActions.Count > 0)
            {
                _screen.Display("Enter your action");
                try
                {
                    DoAction(info);
                }
                catch (AsciiException ex)
                {
                    _screen.Display(ex.Message);
                }
            }
            else
            {
                _screen.Display("Wait for other players to execute their move");
                Task.Delay(500).Wait();
                _screen.Validate();
            }
            _previousGameView = info;
        }

        private void DoAction(GameView info)
        {
            var action = _screen.GetValue();
            if(!int.TryParse(action, out int result)) throw new AsciiException("Invalide actie");
            if(result < 0 || info.You.PossibleActions.Count >= result) throw new AsciiException("Det dachten we efkes nit he!");
            _client.GameAction(_stateMachine.ActiveGame, new PlayView(info.You.PossibleActions[result].Cards));
        }

        private void ShowGameState(GameView info)
        {
            if (!ViewContainsNewData(info)) return;
            _screen.Clear();
            _screen.Display("Center cards:");
            _screen.Display(info.CenterCard);
            _screen.Display("Your hand:");
            _screen.Display(info.You.CardsInHand);
            if (info.You.PossibleActions.Count > 0)
            {
                _screen.Display("Choose your option to play:");
                for (var i = 0; i < info.You.PossibleActions.Count; i++)
                {
                    var cards = string.Join(" ", info.You.PossibleActions[i].Cards);
                    _screen.Display($"{i}: {cards}");
                }
            }
        }

        private bool ViewContainsNewData(GameView info)
        {
            if (_previousGameView == null) return true;
            if (info.CenterCard != _previousGameView.CenterCard) return true;
            if (info.You.PossibleActions.Count != _previousGameView.You.PossibleActions.Count) return true;
            return false;
        }
    }
}