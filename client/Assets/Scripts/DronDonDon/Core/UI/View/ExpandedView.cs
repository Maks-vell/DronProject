using System.Collections.Generic;
using AgkUI.Screens.Service;
using DG.Tweening;
using IoC.Attribute;
using JetBrains.Annotations;
using UnityEngine;

namespace DronDonDon.Core.UI.View
{
    public abstract class ExpandedView : MonoBehaviour, IUIView
    {
        public const float VIEW_SWITCH_TIME = 0.6f;

        [Inject]
        private ScreenStructureManager _screenStructureManager;

        [PublicAPI]
        protected float PanelShowHideTime { get; set; } = VIEW_SWITCH_TIME;

        private readonly List<RectTransform> _leftContainers = new List<RectTransform>();
        private readonly List<RectTransform> _rightContainers = new List<RectTransform>();
        private readonly List<RectTransform> _topContainers = new List<RectTransform>();
        private readonly List<RectTransform> _bottomContainers = new List<RectTransform>();

        [PublicAPI]
        public void RegisterLeftContainer(RectTransform target)
        {
            _leftContainers.Add(target);
        }

        [PublicAPI]
        public void RegisterRightContainer(RectTransform target)
        {
            _rightContainers.Add(target);
        }

        [PublicAPI]
        public void RegisterTopContainer(RectTransform target)
        {
            _topContainers.Add(target);
        }

        [PublicAPI]
        public void RegisterBottomContainer(RectTransform target)
        {
            _bottomContainers.Add(target);
        }

        public virtual void Activate()
        {
            DOTween.Kill(transform);
            foreach (RectTransform target in _leftContainers) {
                DOTween.Kill(target);
                target.gameObject.SetActive(true);
                float targetX = target.pivot.x < 0.5f ? 0 : target.sizeDelta.x;
                target.DOAnchorPosX(targetX, PanelShowHideTime);
            }
            foreach (RectTransform target in _rightContainers) {
                DOTween.Kill(target);
                target.gameObject.SetActive(true);
                float targetX = target.pivot.x < 0.5f ? -target.sizeDelta.x : 0;
                target.DOAnchorPosX(targetX, PanelShowHideTime);
            }
            foreach (RectTransform target in _topContainers) {
                DOTween.Kill(target);
                target.gameObject.SetActive(true);
                float targetY = target.pivot.y < 0.5f ? -target.sizeDelta.y : 0;
                target.DOAnchorPosY(targetY, PanelShowHideTime);
            }
            foreach (RectTransform target in _bottomContainers) {
                DOTween.Kill(target);
                target.gameObject.SetActive(true);
                float targetY = target.pivot.y < 0.5f ? 0 : target.sizeDelta.y;
                target.DOAnchorPosY(targetY, PanelShowHideTime);
            }
        }

        public virtual void Deactivate()
        {
            Vector2 safeAreaOffset = _screenStructureManager.ScreenLayout.SafeAreaScreenOffset
                                     + _screenStructureManager.ScreenLayout.AspectAreaScreenOffset;
            foreach (RectTransform target in _leftContainers) {
                DOTween.Kill(target);
                float targetX = target.pivot.x < 0.5f ? -target.sizeDelta.x : 0;
                target.DOAnchorPosX(targetX - safeAreaOffset.x, PanelShowHideTime).OnComplete(() => target.gameObject.SetActive(false));
            }
            foreach (RectTransform target in _rightContainers) {
                DOTween.Kill(target);
                float targetX = target.pivot.x < 0.5f ? 0 : target.sizeDelta.x;
                target.DOAnchorPosX(targetX + safeAreaOffset.x, PanelShowHideTime).OnComplete(() => target.gameObject.SetActive(false));
            }
            foreach (RectTransform target in _topContainers) {
                DOTween.Kill(target);
                float targetY = target.pivot.y < 0.5f ? 0 : target.sizeDelta.y;
                target.DOAnchorPosY(targetY - safeAreaOffset.y, PanelShowHideTime).OnComplete(() => target.gameObject.SetActive(false));
            }
            foreach (RectTransform target in _bottomContainers) {
                DOTween.Kill(target);
                float targetY = target.pivot.y < 0.5f ? -target.sizeDelta.y : 0;
                target.DOAnchorPosY(targetY + safeAreaOffset.y, PanelShowHideTime).OnComplete(() => target.gameObject.SetActive(false));
            }
            transform.DOScaleZ(1, PanelShowHideTime + 0.1f).OnComplete(OnDeactivated);
        }

        protected virtual void OnDeactivated()
        {
        }
    }
}