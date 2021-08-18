using IngameDebugConsole.Console.Service;
using IoC.Api;

namespace DronDonDon.Console.IoC
{
    public class ConsoleModule : IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<IConsoleService, IngameConsoleService>();
            
        }
    }
}