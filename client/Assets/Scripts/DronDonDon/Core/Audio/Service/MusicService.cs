using Adept.Logger;
using AgkCommons.CodeStyle;
using AgkCommons.Resources;
using IoC.Attribute;
using JetBrains.Annotations;
using UnityEngine;

namespace DronDonDon.Core.Audio.Service
{
    [Injectable]
    public class MusicService : MonoBehaviour
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<MusicService>();

        [Inject]
        private ResourceService _resourceService;
        [Inject]
        private AudioService _audioService;

        private string _lastMusicPrefab;
        private bool _musicEnabled;

        [PublicAPI]
        public void PlayMusic(string prefab)
        {
            _resourceService.LoadResource<AudioClip>(prefab, OnMusicLoaded, prefab);
            _lastMusicPrefab = prefab;
        }
        [PublicAPI]
        public void StopMusic()
        {
            _audioService.StopMusic();
        }
        [PublicAPI]
        public void PauseMusic()
        {
            _audioService.PauseMusic();
        }
        
        [PublicAPI]
        public void ResumeMusic()
        {
            if (string.IsNullOrEmpty(_lastMusicPrefab)) {
                return;
            }
            PlayMusic(_lastMusicPrefab);
        }
        private void OnMusicLoaded(AudioClip loadedObject, object[] loadParameters)
        {
            string prefabName = loadParameters[0] as string;
            if (loadedObject == null) {
                _logger.Warn("Music not loaded. id=" + prefabName);
                return;
            }

            if (!_lastMusicPrefab.Equals(prefabName)) {
                return;
            }

            _audioService.PlayMusic(loadedObject);
        }

        public bool MusicEnabled
        {
            set
            {
                if (_musicEnabled == value) {
                    return;
                }
                _musicEnabled = value;
                if (_musicEnabled) {
                    ResumeMusic();
                } else {
                    StopMusic();
                }
            }
        }
    }
}