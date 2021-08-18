using AgkUI.Screens.Service;
using DronDonDon.MainMenu.UI.Screen;
using IoC.Attribute;
using IoC.Util;

namespace DronDonDon.Core.Filter
{
    public class InitScreenFilter : IAppFilter
    {
        [Inject]
        private ScreenManager _screenManager;
        [Inject]
        private IoCProvider<OverlayManager> _overlayManager;

        public void Run(AppFilterChain chain)
        {
            _screenManager.LoadScreen<InitScreen>();
            _overlayManager.Require().ShowPreloader();
            chain.Next();
        }
    }
}