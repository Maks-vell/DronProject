using Adept.Logger;
using AgkUI.Binding.Attributes;
using DronDonDon.Core.UI.Builder;
using DronDonDon.Core.UI.View;
using DronDonDon.Game.Levels.UI;
using DronDonDon.MainMenu.UI.Panel;

// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Local

namespace DronDonDon.MainMenu.UI.View
{
    [UIController(PREFAB)]
    public class StartGameView : ExpandedView
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<StartGameView>();

        private const string PREFAB = "StartGameView";

        private MainMenuPanel _mainMenuPanel;

        [UICreated]
        public void Init()
        {
        }

        
        public void Load(UIScreenPanelLoader uiScreenPanelLoader)
        {
            uiScreenPanelLoader.Add<MainMenuPanel>((mainMenu) => {
                _mainMenuPanel = mainMenu;
                _mainMenuPanel.Init();
            });
        }
    }
}