using System;
using System.Collections.Generic;
using System.Linq;
using AgkCommons.Extension;
using AgkCommons.Util;
using AgkUI.Core.Model;
using AgkUI.Core.Service;
using AgkUI.Screens.Service;
using IoC.Attribute;
using IoC.Extension;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace DronDonDon.Core.UI.Builder
{
    public class UIScreenPanelLoader : MonoBehaviour
    {
        [Inject]
        private UIService _uiService;
        [Inject]
        private ScreenStructureManager _screenStructureManager;

        [CanBeNull]
        private UnityAction _onLoadComplete;
        private readonly List<PrefabData> _items = new List<PrefabData>();
        private int _loadedItems;

        private int _currentDepth;

        private void Awake()
        {
            this.Inject();
        }

        [NotNull]
        public UIScreenPanelLoader Add<T>([CanBeNull] UnityAction<T> loadCallback = null)
                where T : class
        {
            _items.Add(new PrefabData(typeof(T), new CallbackObject<object>(go => {
                if (typeof(T) == typeof(GameObject)) {
                    loadCallback?.Invoke((T) go);
                    return;
                }
                GameObject loadObject = (GameObject) go;
                if (loadObject == null) {
                    loadCallback?.Invoke(null);
                    return;
                }
                loadCallback?.Invoke(loadObject.GetComponent<T>());
            })));
            return this;
        }

        public void Load([CanBeNull] UnityAction completeCallback)
        {
            _onLoadComplete = completeCallback;

            List<PrefabData> panels = _items.ToList();
            foreach (PrefabData prefabData in panels) {
                _uiService.Create(UiModel.Create(prefabData.Prefab)).Then(p => {
                    if (this.IsDestroyed()) {
                        Destroy(p);
                        return;
                    }
                    OnPrefabLoaded(p.gameObject, prefabData, panels.Count - panels.IndexOf(prefabData));
                });
            }
        }

        private void OnPrefabLoaded([CanBeNull] GameObject uiElement, PrefabData prefabData, int depth)
        {
            if (uiElement != null) {
                _screenStructureManager.AttachToSafeArea(uiElement, depth);
            }
            prefabData.CallbackObject.Invoke(uiElement);
            _items.Remove(prefabData);

            if (_items.Count != 0) {
                return;
            }
            _onLoadComplete?.Invoke();
            Destroy(this);
        }

        private struct PrefabData
        {
            public Type Prefab { get; }
            public CallbackObject<object> CallbackObject { get; }

            public PrefabData(Type prefab, CallbackObject<object> callbackObject)
            {
                Prefab = prefab;
                CallbackObject = callbackObject;
            }
        }
    }
}