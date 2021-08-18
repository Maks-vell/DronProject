using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Dialog.Attributes;
using AgkUI.Dialog.Service;
using AgkUI.Element.Buttons;
using DronDonDon.Core.UI.Dialog;
using IoC.Attribute;
using IoC.Util;
using Adept.Logger;
using DronDonDon.Settings.Service;
using UnityEngine;

namespace DronDonDon.Settings.UI
{
    [UIController("UI/Dialog/pfSettingsDialog@embeded")]
    [UIDialogFog(FogPrefabs.EMBEDED_SHADOW_FOG)]
    public class GameSettingsDialog : MonoBehaviour
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<GameSettingsDialog>();

        [UIComponentBinding("SoundToggleButton")]
        private ToggleButton _toggleSoundButton;
        
        [UIComponentBinding("MusicToggleButton")]
        private ToggleButton _toggleMusicButton;
        
        [Inject] 
        private SettingsService _settingsService;
        
        [Inject]
        private IoCProvider<DialogManager> _dialogManager;
        private void Start()
        {
            _toggleMusicButton.IsOn = _settingsService.GetMusicMute();
            _toggleSoundButton.IsOn = _settingsService.GetSoundMute();
        }
        
        [UIOnClick("CloseButton")]
        private void CloseButton()
        {
            _dialogManager.Require()
                .Hide(gameObject);
        }
        
        [UIOnClick("SoundToggleButton")]
        private void OnSoundButton()
        {
            _logger.Debug("MuteSound");
            _settingsService.SetSoundMute(_toggleSoundButton.IsOn);
        }
        
        [UIOnClick("MusicToggleButton")]
        private void OnMusicButton()
        {
            _logger.Debug("MuteMusic");
            _settingsService.SetMusicMute(_toggleMusicButton.IsOn);
        }

        [UIOnClick("ResetButton")]
        private void OnResetButton()
        {
            _logger.Debug("Reset");
            _settingsService.ResetAllProgress();
            
        }

        [UIOnClick("Info")]
        private void OnInfoClick()
        {
            _dialogManager.Require().ShowModal<DownloadedDialog>();
        }
    }
}