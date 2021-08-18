using Adept.Logger;
using AgkCommons.Event;
using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Dialog.Attributes;
using AgkUI.Dialog.Service;
using AgkUI.Element.Text;
using AgkUI.Screens.Service;
using DronDonDon.Core.Audio;
using DronDonDon.Core.Audio.Model;
using DronDonDon.Core.Audio.Service;
using DronDonDon.Core.UI.Dialog;
using DronDonDon.Game.Levels.Service;
using DronDonDon.MainMenu.UI.Screen;
using IoC.Attribute;
using IoC.Util;

namespace DronDonDon.Game.LevelDialogs
{
    [UIController(PREFAB_NAME)]
    [UIDialogFog(FogPrefabs.EMBEDED_SHADOW_FOG)]
    public class LevelFailedCompactDialog : GameEventDispatcher
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<LevelFailedCompactDialog>();
        private const string PREFAB_NAME = "UI/Dialog/pfLevelFailedCompactDialog@embeded";

        private string _levelId;
        private short _failReason = 0;
        // "Закончилась энергия"
        // "Дрон разбился"
        
        [Inject]
        private IoCProvider<DialogManager> _dialogManager;
        
        [Inject]
        private ScreenManager _screenManager;
        
        [Inject]
        private LevelService _levelService;
        
        [Inject] 
        private SoundService _soundService;
        
        [UIComponentBinding("FailReasonTitle")]
        private UILabel _failReasonLabel;
        
        [UICreated]
        public void Init(short failReason)
        {
            _logger.Debug("[LevelFailedCompactDialog] Init()...");
            _levelId = _levelService.CurrentLevelId;

            _failReason = failReason;
            SetDialogLabels();
            PlaySound(GameSounds.FAILED);
        }
        
        private void PlaySound(Sound sound)
        {
            _soundService.StopAllSounds();
            _soundService.PlaySound(sound);
        }
        
        [UIOnClick("RestartButton")]
        private void RestartButtonClicked()
        {
            _dialogManager.Require().Hide(this);
            _screenManager.LoadScreen<MainMenuScreen>();
            _levelService.ShowStartLevelDialog(_levelId);
        }

        [UIOnClick("LevelMapButton")]
        private void LevelMapButtonClicked()
        {
            _dialogManager.Require().Hide(this);
            _screenManager.LoadScreen<MainMenuScreen>();
        }

        private void SetDialogLabels()
        {
            switch (_failReason)
            {
                case 0: _failReasonLabel.text = "Дрон разбился";
                    break;
                case 1: _failReasonLabel.text = "Закончилась энергия";
                    break;
                default: _failReasonLabel.text = "Дрон разбился";
                    break;
            }
        }
    }
}