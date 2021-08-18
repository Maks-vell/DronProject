using System.Collections;
using System.Collections.Generic;
using AgkCommons.CodeStyle;
using JetBrains.Annotations;
using UnityEngine;

namespace DronDonDon.Core.Audio.Service
{
    [Injectable]
    public class AudioService : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _music1Source;

        [SerializeField]
        private AudioSource _music2Source;
        [SerializeField]
        private AudioListener _menuAudioListener;
        [SerializeField]
        [Range(1, 10)]
        private float _musicCrossFadeRate = 3f;
        
        private Dictionary<int, AudioSource> _tracks = new Dictionary<int, AudioSource>();

        private AudioSource _activeMusic;
        private AudioSource _inactiveMusic;

        private bool _musicCrossFading;
        private Coroutine _crossFadingCoroutine;
        private bool _musicMute;
        private bool _sound3D;

        private bool _soundMute;

        private void Awake()
        {
            /*_music2Source.clip = Resources.Load("Embeded/Audio/Music/mainMenuMusic", typeof(AudioClip))as AudioClip;
            _music1Source.clip = Resources.Load("Embeded/Audio/Music/gameMusic", typeof(AudioClip))as AudioClip;
            _music1Source.volume = 0f;
            _music2Source.volume = 0f;*/
            _activeMusic = _music1Source;
            _inactiveMusic = _music2Source;
            PlayMusic(_activeMusic.clip);
        }

        public void PlaySound(AudioClip clip, int track = 0, bool loop = false)
        {
            if (!_tracks.ContainsKey(track)) {
                GameObject audioSourceObject = new GameObject(clip.name);
                audioSourceObject.transform.SetParent(gameObject.transform);
                AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
                _tracks.Add(track, audioSource);
            }
            _tracks[track].clip = clip;
            
            if (!loop) {
                _tracks[track].PlayOneShot(clip);
                return;
            }
            _tracks[track].loop = true;
            _tracks[track].Play();
        }


        public void StopSound(int trackNumber = 0)
        {
            if (!_tracks.ContainsKey(trackNumber)) {
                return;
            }
            _tracks[trackNumber].Stop();
            Destroy(_tracks[trackNumber].gameObject);
            _tracks.Remove(trackNumber);
        }

        public void PauseSound()
        {
            foreach (KeyValuePair<int, AudioSource> audioSourceKeyValue in _tracks) {
                audioSourceKeyValue.Value.Pause();
            }
        }

        public void ResumeSound()
        {
            foreach (KeyValuePair<int, AudioSource> audioSourceKeyValue in _tracks) {
                audioSourceKeyValue.Value.UnPause();
            }
        }

        public void PlayMusic(AudioClip clip, float offset = 0f)
        {
            if (_musicCrossFading) {
                StopCoroutine(_crossFadingCoroutine);
            }
            _crossFadingCoroutine = StartCoroutine(CrossFadeMusic(clip, offset));
        }


        [PublicAPI]
        public void EnableMenuAudioListener()
        {
            _menuAudioListener.gameObject.SetActive(true);
        }

        [PublicAPI]
        public void DisableMenuAudioListener()
        {
            _menuAudioListener.gameObject.SetActive(false);
        }

        [PublicAPI]
        public void PauseMusic()
        {
            _inactiveMusic.Pause();
            _activeMusic.Pause();
            _musicCrossFading = false;
        }

        [PublicAPI]
        public void ResumeMusic()
        {
            _inactiveMusic.UnPause();
            _activeMusic.UnPause();
        }
        [PublicAPI]
        public void UnMute()
        {
            AudioListener.volume = 1.0f;
        }

        [PublicAPI]
        public void Mute()
        {
            AudioListener.volume = 0f;
        }

        [PublicAPI]
        public void StopMusic()
        {
            _activeMusic.Stop();
            _inactiveMusic.Stop();
            if (_crossFadingCoroutine != null) {
                StopCoroutine(_crossFadingCoroutine);
            }
            _musicCrossFading = false;
        }

        [PublicAPI]
        public void StopAllSounds()
        {
            foreach (KeyValuePair<int, AudioSource> audioSourceKeyValue in _tracks) {
                audioSourceKeyValue.Value.Stop();
                Destroy(audioSourceKeyValue.Value.gameObject);
            }
            _tracks.Clear();
        }

        public void FadeAndStopMusic()
        {
            if (_musicCrossFading) {
                return;
            }
            StartCoroutine(FadeMusic());
        }

        private IEnumerator FadeMusic()
        {
            _musicCrossFading = true;

            float scaleRate = 1 / _musicCrossFadeRate;
            while (_activeMusic.volume > 0) {
                _activeMusic.volume -= scaleRate * Time.deltaTime;
                yield return null;
            }
            _activeMusic.Stop();
            _musicCrossFading = false;
        }

        private IEnumerator CrossFadeMusic(AudioClip clip, float offset)
        {
            _musicCrossFading = true;
            _inactiveMusic.clip = clip;
            _inactiveMusic.volume = 0;
            _inactiveMusic.time = offset;
            _activeMusic.mute = _musicMute;
            _inactiveMusic.Play();

            float scaleRate = 1 / _musicCrossFadeRate;

            while (_activeMusic.volume > 0) {
                _activeMusic.volume -= scaleRate * Time.deltaTime;
                _inactiveMusic.volume += scaleRate * Time.deltaTime;

                yield return new WaitForSecondsRealtime(0.0001f);
            }

            AudioSource temp = _activeMusic;
            _activeMusic = _inactiveMusic;
            _activeMusic.volume = 0.090f;

            _inactiveMusic = temp;
            _inactiveMusic.Stop();

            _musicCrossFading = false;
        }

        [PublicAPI]
        public bool SoundMute
        {
            get { return _soundMute; }
            set
            {
                _soundMute = value;
                if (_tracks.Count == 0) {
                    return;
                }
                foreach (KeyValuePair<int, AudioSource> audioSource in _tracks) {
                    audioSource.Value.mute = _soundMute;
                }
            }
        }

        [PublicAPI]
        public bool MusicMute
        {
            get { return _musicMute; }
            set
            {
                _musicMute = value;
                if (_music1Source != null) {
                    _music1Source.mute = _musicMute;
                }
                if (_music2Source != null) {
                    _music2Source.mute = _musicMute;
                }
            }
        }
    }
}