using DronDonDon.Core.Audio.Service;
using DronDonDon.Core.Configurations;
using IoC.Attribute;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DronDonDon.Core.Filter
{
    public class AppSettingsFilter : IAppFilter
    {
        [Inject]
        private AudioService _audioService;

        public void Run(AppFilterChain chain)
        {
            InitSleepSettings();
            InitAudioSettings();
            InitGraphicsQualitySettings();
            InitPixelDragThreshold();
            InitGlobalException(chain.gameObject);

            chain.Next();
        }

        private void InitSleepSettings()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void InitPixelDragThreshold()
        {
            int pixelDragThreshold = EventSystem.current.pixelDragThreshold;
            EventSystem.current.pixelDragThreshold = Mathf.Max(pixelDragThreshold, (int) (pixelDragThreshold * (Screen.dpi / 160f)));
        }

        private void InitAudioSettings()
        {
            _audioService.SoundMute = PlayerPrefsConfig.SoundMute;
            _audioService.MusicMute = PlayerPrefsConfig.MusicMute;
        }
        

        private void InitGraphicsQualitySettings()
        {
            // string[] names = QualitySettings.names;
            //
            // int configLevel = Array.IndexOf(names, Config.GraphicsQualityLevel);
            // if (configLevel != -1 && QualitySettings.GetQualityLevel() != configLevel) {
            //     QualitySettings.SetQualityLevel(configLevel);
            // }
        }

        private void InitGlobalException(GameObject gameObject)
        {
            gameObject.AddComponent<ExceptionHandler>();
        }
    }
}