using AgkCommons.Util;
using AgkUI.Element.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace DronDonDon.Resource.UI.Element.Progress
{
    public class FilledImageProgressBar : MonoBehaviour, IProgressBar
    {
        private const float ROUNDING_THRESHOLD = 0.1f;
        private const int MAX_PROGRESS = 100;

        [SerializeField]
        private UILabel _label;
        [SerializeField]
        [Range(0f, 1f)]
        private float _speed = 0.1f;

        private int _progress;
        private float _visibleProgress;

        private void Update()
        {
            VisibleProgress = Mathf.Lerp(VisibleProgress, Progress, Speed);
        }

        public void SetProgressImmediately(int progress)
        {
            VisibleProgress = progress;
            Progress = progress;
        }

        public int Progress
        {
            get { return _progress; }
            set { _progress = value; }
        }

        [PublicAPI]
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public bool Completed
        {
            get { return MathUtils.IsFloatEquals(VisibleProgress, MAX_PROGRESS); }
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

        private float VisibleProgress
        {
            get { return _visibleProgress; }
            set
            {
                if (Mathf.Abs(value - Progress) <= ROUNDING_THRESHOLD) {
                    value = Progress;
                }
                _visibleProgress = value;
                GetComponent<Image>().fillAmount = _visibleProgress / 100f;
            }
        }
    }
}