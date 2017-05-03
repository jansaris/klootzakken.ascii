using Klootzakken.Api.Model;

namespace Klootzakken.Ascii
{
    public interface IStateMachine
    {
        string ActiveGame { get; }
        StateMachine.States State { get; }
        void LoggedIn(string token);
        void JoinedGame(string gameId);
        void ExitGame();
    }
}