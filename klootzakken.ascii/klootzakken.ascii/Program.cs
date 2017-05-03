using System;
using System.Reflection;
using Klootzakken.Api.Client;
using Ninject;

namespace Klootzakken.Ascii
{
    public class Program : IProgram
    {
        private readonly IScreen _screen;
        private readonly IStateMachine _stateMachine;
        private readonly ILobby _lobby;
        private readonly IGame _game;

        public Program(IScreen screen, IStateMachine stateMachine, ILobby lobby, IGame game)
        {
            _screen = screen;
            _stateMachine = stateMachine;
            _lobby = lobby;
            _game = game;
        }

        static void Main()
        {
            try
            {
                var kernel = new StandardKernel();
                kernel.Load(Assembly.GetExecutingAssembly());
                var program = kernel.Get<IProgram>();
                program.Run();
            }
            catch (ApiException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine($"Wahhhhhhhhh the server returned a {ex.ErrorCode}: {ex.Message}");
                Console.WriteLine("");
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine($"Wahhhhhhhhh: {ex.Message}");
                Console.WriteLine("");
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
        }

        public void Run()
        {
            _screen.Display("Welcome to Klootzakken ASCII");

            _screen.Log("Start main loop!");
            _screen.Exit += Exit;
            _screen.Display("Type 'exit' to exit the application");
            while (true)
            {
                switch (_stateMachine.State)
                {
                     case StateMachine.States.Login:
                        _screen.Display("Enter a token to continue:");
                        _stateMachine.LoggedIn("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI5NmM4N2E2MC1hNWEzLTRjMGUtYjA1Ny0wYTlmOTU4NDZlZGUiLCJ1bmlxdWVfbmFtZSI6InJvYmJlcnQuZHJpZXNzZW5AaGlnaHRlY2hpY3QubmwiLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6IjliNjYzMDNjLWI5MjAtNDdkZS1hMDFmLTMxZWMxOGQwOWEyZCIsIm5iZiI6MTQ5MzgzMDg3NiwiZXhwIjoxNDk2NTA5Mjc2LCJpYXQiOjE0OTM4MzA4NzYsImlzcyI6IkRpdnZlcmVuY2UuY29tIEtsb290emFra2VuIiwiYXVkIjoiRGVtb0F1ZGllbmNlIn0.1Q5BSz3xZXebNvddSG4duG0cWLwA-4Ao1DILHsPGu_k");
                         break;
                    case StateMachine.States.Lobby:
                        _lobby.Do();
                        break;
                    case StateMachine.States.InGame:
                        _game.Do();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool Exit()
        {
            _screen.Display("Haije");
            Environment.Exit(1);
            return true;
        }
    }
}
