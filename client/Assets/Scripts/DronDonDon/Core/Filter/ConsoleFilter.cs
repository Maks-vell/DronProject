using DronDonDon.Core.Configurations;
using IngameDebugConsole.Console.Service;
using IoC.Attribute;
using IoC.Util;

namespace DronDonDon.Core.Filter
{
    public class ConsoleFilter : IAppFilter
    {
        [Inject]
        private IoCProvider<IConsoleService> _console;

        public void Run(AppFilterChain chain)
        {
            if (Config.ShowConsole) {
                _console.Require().Create();
            }
            chain.Next();
        }
    }
}