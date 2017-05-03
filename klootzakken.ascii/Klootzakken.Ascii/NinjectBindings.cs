using Klootzakken.Api.Api;
using Ninject.Modules;

namespace Klootzakken.Ascii
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IProgram>().To<Program>();
            Bind<IScreen>().To<Screen>().InSingletonScope();
            Bind<IStateMachine>().To<StateMachine>().InSingletonScope();
            Bind<ILobby>().To<Lobby>();
            Bind<IGame>().To<Game>();
            var gameApi = new GameApi("http://www.glueware.nl/Klootzakken/kzapi");
            Bind<GameApi>().ToConstant(gameApi);
        }
    }
}
