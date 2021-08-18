using AgkUI.Dialog.Service;
using AgkUI.Screens.Service;
using AgkUI.Screens.Ui;
using DronDonDon.Core.UI.Dialog.Service;
using DronDonDon.World;
using IoC;
using IoC.Api;
using IoC.Scope;
using JetBrains.Annotations;
using UnityEngine;

namespace DronDonDon.Core.UI.IoC
{
    public class UIModule : IIoCModule
    {
        private const string DIALOG_CONTAINER_NAME = "DialogContainer";
        private const string OVERLAY_CONTAINER_NAME = "OverlayContainer";
        
        private const int DIALOG_CONTAINER_DEPTH = 10;

        private ScreenStructureManager _screenStructureManager;

        public void Configure([NotNull] IIoCContainer container)
        {
            container.RegisterSingleton<OverlayManager>(CreateOverlayManager);
            container.RegisterSingleton<ScreenManager>();
            container.RegisterSingleton<DialogService>();

            container.RegisterSingleton<ScreenStructureManager>();
            container.RegisterSingleton<GameWorld>(GetMainWorld, null, ScopeType.SCREEN);
            container.RegisterSingleton<DialogManager>(CreateDialogManager, null, ScopeType.SCREEN);
            _screenStructureManager = AppContext.Resolve<ScreenStructureManager>();
        }

        [NotNull]
        private DialogManager CreateDialogManager()
        {
            GameObject dialogContainer = CreateContainer(DIALOG_CONTAINER_NAME, DIALOG_CONTAINER_DEPTH);
            DialogManager dialogManager = dialogContainer.AddComponent<DialogManager>();
            AppContext.Resolve<ScreenStructureManager>().AttachToAspect21Screen(dialogManager.gameObject);
            return dialogManager;
        }

        [NotNull]
        private OverlayManager CreateOverlayManager()
        {
            GameObject overlayContainer = GameObject.Find(OVERLAY_CONTAINER_NAME);
            OverlayManager overlayManager = overlayContainer.AddComponent<OverlayManager>();
            return overlayManager;
        }

        [NotNull]
        private static GameObject CreateContainer(string name, int depth = 0)
        {
            GameObject container = new GameObject(name);
            UIDepth depthComponent = container.AddComponent<UIDepth>();
            depthComponent.Depth = depth;
            RectTransform rectTransform = container.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            return container;
        }
        
        [CanBeNull]
        private object GetMainWorld()
        {
            if (_screenStructureManager.ScreenWorldViewContainer == null) {
                return null;
            }
            return _screenStructureManager.ScreenWorldViewContainer.GetComponentInChildren<GameWorld>();
        }
    }
}