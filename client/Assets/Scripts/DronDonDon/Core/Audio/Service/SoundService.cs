using System.Collections.Generic;
using System.Linq;
using Adept.Logger;
using AgkCommons.CodeStyle;
using AgkCommons.Resources;
using DronDonDon.Core.Audio.Model;
using DronDonDon.Game;
using IoC.Attribute;
using JetBrains.Annotations;
using UnityEngine;
using DronDonDon.Core.Audio;

namespace DronDonDon.Core.Audio.Service
{
    [Injectable]
    public class SoundService : MonoBehaviour
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<SoundService>();


        [Inject]
        private readonly ResourceService _resourceService;
        [Inject]
        private readonly AudioService _audioService;

        private readonly IDictionary<string, AudioClip> _soundClips = new Dictionary<string, AudioClip>();
        private List<Sound> _sounds = new List<Sound>();

        private void Start()
        {
            LoadEmbededSounds();
        }

        [PublicAPI]
        public void PlaySound(Sound sound)
        {
            string actualSoundName = sound.SoundName;

            if (!_soundClips.ContainsKey(actualSoundName)) {
                _logger.Warn("Sound not found: name=" + actualSoundName);
                return;
            }
            if (IsClipPlaying(sound.SoundName)) {
                return;
            }
            AudioClip clip = _soundClips[actualSoundName];
            
            _sounds.Add(sound);
            foreach (Sound item in _sounds)
            {
            }
            
            _audioService.PlaySound(clip, sound.Track, sound.Looped);
        }

        [PublicAPI]
        public void StopSound(Sound sound)
        {
            if (!IsClipPlaying(sound.SoundName)) {
                _logger.Debug("Can't play sound. It Doesnt exists ");
                return;
            }
            Sound playingSound = _sounds.FirstOrDefault(p => p.SoundName == sound.SoundName);
            _audioService.StopSound(playingSound.Track);
            _sounds.Remove(playingSound);
        }

        [PublicAPI]
        public void PauseSound()
        {
            _audioService.PauseSound();
        }

        [PublicAPI]
        public void ResumeSound()
        {
            _audioService.ResumeSound();
        }

        [PublicAPI]
        public void StopAllSounds()
        {
            _sounds.Clear();
            _audioService.StopAllSounds();
        }

        private bool IsClipPlaying(string name)
        {
            return _sounds.FirstOrDefault(p => p.SoundName == name) != null;
        }

        private void LoadEmbededSounds()
        {
            List<Sound> embededSounds = new List<Sound> {
                    GameSounds.CHIP_PICKUP,
                    GameSounds.BOOSTER_PICKUP,
                    GameSounds.SHIELD_ACTIVATED,
                    GameSounds.SPEED_ACTIVATED,
                    GameSounds.COLLISION,
                    GameSounds.DRON_CRASHED,
                    GameSounds.DRON_TAKEOFF,
                    GameSounds.DRON_LANDING,
                    GameSounds.SHOW_DIALOG,
                    GameSounds.SHIFT,
                    GameSounds.FAILED,
                    GameSounds.VICTORY
            };
            foreach (Sound sound in embededSounds) {
                string soundName = sound.SoundName;
              
                _resourceService.LoadAudioClip(sound.SoundPath, (loadedClip, loadedParams) => {
                    if (loadedClip == null) {
                        _logger.Warn("Sound not loaded: prefab=" + sound.SoundPath);
                        return;
                    }
                    if (_soundClips.ContainsKey(soundName)) {
                        return;
                    }
                    loadedClip.name = soundName;
                    _soundClips.Add(soundName, loadedClip);
                });
            }
        }
    }

    
}