using System;
using Adept.Logger;
using AgkUI.Element.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DronDonDon.Resource.UI.Element.Progress
{
    public class SimpleProgressBar : MonoBehaviour, IProgressBar
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<SimpleProgressBar>();

        private const float ROUNDING_THRESHOLD = 0.1f;
        private const float NORMALIZATION_COEFF = 100f;
        private const float MIN_SPEED = 0.001f;

        [FormerlySerializedAs("slider")]
        [SerializeField]
        private Slider _slider;

        [FormerlySerializedAs("speed")]
        [SerializeField]
        [Range(MIN_SPEED, 100f)]
        private float _speed = 0.1f;

        [FormerlySerializedAs("progress")]
        [SerializeField]
        [Range(0, 100)]
        private int _progress;

        [FormerlySerializedAs("label")]
        [SerializeField]
        private UILabel _label;

        private float _visibleProgress;

        private void Update()
        {
            VisibleProgress = Mathf.Clamp(VisibleProgress + _speed * NORMALIZATION_COEFF * Time.deltaTime, 0, Progress);
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                if (_progress == 0) {
                    VisibleProgress = 0;
                }
            }
        }

        private float VisibleProgress
        {
            get { return _visibleProgress; }
            set
            {
                if (Mathf.Abs(value - Progress) <= ROUNDING_THRESHOLD) {
                    value = Progress;
                }
                _slider.value = _visibleProgress = value;
            }
        }

        public bool Completed
        {
            get { return Math.Abs(VisibleProgress - 100) < 0.001f; }
        }

        public float Speed
        {
            get { return _speed; }
            set
            {
                if (value < MIN_SPEED) {
                    _logger.Debug("Try to set negative or too low speed");
                    _speed = MIN_SPEED;
                }
                _speed = value;
            }
        }

        public string Label
        {
            get
            {
                if (_label == null) {
                    _label = GetComponentInChildren<UILabel>();
                }
                return _label != null ? _label.StyledText : null;
            }
            set
            {
                if (_label == null) {
                    _label = GetComponentInChildren<UILabel>();
                }
                if (_label != null) {
                    _label.StyledText = string.IsNullOrEmpty(value) ? "" : value;
                }
            }
        }
    }
}