using System.Runtime.InteropServices;
using AgkUI.Core.Model;
using AgkUI.Core.Service;
using DronDonDon.Core.UI.Overlay;
using DronDonDon.Game.LevelDialogs;
using DronDonDon.Location.UI;
using IoC.Attribute;
using DronDonDon.Core.UI.Overlay;

using UnityEngine;

namespace DronDonDon.Core
{
    public class OverlayManager : MonoBehaviour
    {
        private int _lockCount;
        private PreloaderOverlay _preloaderOverlay;

        [Inject] 
        private UIService _uiService;

        private DronStatsDialog _dronStats;
        
        private LevelFinishedDialog _finishedDialog=null;

        private LevelFailedCompactDialog _failedDialog=null;

        private GameObject levelContainer;
        
        private void Awake()
        {
            _preloaderOverlay = FindObjectOfType<PreloaderOverlay>();
        }

        public void HideLoadingOverlay(bool removePreloaderAfterComplete = false)
        {
            // ReSharper disable once UseNullPropagation
            if (!ReferenceEquals(_preloaderOverlay, null)) {
                _preloaderOverlay.Complete(removePreloaderAfterComplete);
            }
        }
        
        public void CreateGameOverlay(DronStats dronStats)
        { 
           levelContainer = GameObject.Find($"Overlay");
            
            _uiService.Create<DronStatsDialog>(UiModel
                    .Create<DronStatsDialog>(dronStats)
                    .Container(levelContainer))
                .Then(controller => { _dronStats = controller;})
                .Done();
        }
        
        public void DestroyGameOverlay()
        {
            Destroy(_dronStats.gameObject);
            Light lightOnLevel = GameObject.Find("DirectionalLight").GetComponent<Light>();
            lightOnLevel.color = Color.white;
            lightOnLevel.intensity = 1.3f;
        }
        
        public void ShowPreloader()
        {
            PreloaderOverlay.Show();
        }

        public void HidePreloader()
        {
            PreloaderOverlay.Hide();
        }

        private PreloaderOverlay PreloaderOverlay
        {
            get
            {
                if (_preloaderOverlay != null) {
                    return _preloaderOverlay;
                }
                GameObject prefab = Resources.Load("Embeded/Preloader/pfPreloader", typeof(GameObject)) as GameObject;
                GameObject go = Instantiate(prefab, transform);
                _preloaderOverlay = go.GetComponent<PreloaderOverlay>();
                return _preloaderOverlay;
            }
        }
    }
}