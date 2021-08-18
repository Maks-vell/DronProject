using System.Collections;
using AgkUI.Core.Service;
using DronDonDon.Resource.UI.Element.Progress;
using IoC.Attribute;
using IoC.Extension;
using UnityEngine;

namespace DronDonDon.Core.UI.Overlay
{
    public class PreloaderOverlay : MonoBehaviour
    {
        private const float PREDICT_TIME = 8;
        private const float SLOW_FILL_SPEED = 0.1f;
        private const float FAST_FILL_SPEED = 1f;
        private const int MAX_NOT_LOADED_PROGRESS = 95;
        private const int MIN_NOT_LOADED_PROGRESS = 5;
        private const float DECREASING_СOEFFICIENT = 0.5f;
        private const int FIRST_HALF_OF_PROGRESS = 45;
        [Inject]
        private UIService _uiService;
        
        private IProgressBar _progressBar;
        private Coroutine _fillProgressCoroutine;
        private bool _removeAfterComplete;
        private bool _destroyNextFrame;
        private int _progressSummer;

        private void Awake()
        {
            this.Inject();
            _progressBar = gameObject.GetComponent<IProgressBar>();
        }


        private void Update()
        {
            if (!_progressBar.Completed) {
                return;
            }
            if (_destroyNextFrame) {
                if (_removeAfterComplete) {
                    Destroy(gameObject);
                    _removeAfterComplete = false;
                } else {
                    gameObject.SetActive(false);
                }
            }
            _destroyNextFrame = true;
        }

        public void Show()
        {
            if (_fillProgressCoroutine != null) {
                return;
            }
            gameObject.SetActive(true);
            _destroyNextFrame = false;
            _progressBar.Progress = MIN_NOT_LOADED_PROGRESS;
            _progressBar.Speed = SLOW_FILL_SPEED;
            _fillProgressCoroutine = StartCoroutine(FillProgress());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Complete(bool removeAfterComplete)
        {
            if (_fillProgressCoroutine != null) {
                StopCoroutine(_fillProgressCoroutine);
                _fillProgressCoroutine = null;
            }
            _progressBar.Speed = FAST_FILL_SPEED;
            _progressBar.Progress = 100;
            _removeAfterComplete |= removeAfterComplete;
            
        }

        private IEnumerator FillProgress()
        {
            float nextTime = Time.realtimeSinceStartup + PREDICT_TIME;
            float increaseInPercent = FIRST_HALF_OF_PROGRESS / PREDICT_TIME; 
            float currentProgress = MIN_NOT_LOADED_PROGRESS;
            while (true) {
                _progressBar.Progress = Mathf.RoundToInt(Mathf.Clamp(currentProgress, MIN_NOT_LOADED_PROGRESS, MAX_NOT_LOADED_PROGRESS));
                float currentTime = Time.realtimeSinceStartup;
                currentProgress += increaseInPercent;
                if (nextTime < currentTime) {
                    nextTime = currentTime + PREDICT_TIME;
                    increaseInPercent *= DECREASING_СOEFFICIENT;
                }
                yield return null;
            }
        }
    }
}