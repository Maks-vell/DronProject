using AgkUI.Binding.Attributes;
using DronDonDon.Core;
using IoC.Attribute;
using UnityEngine;

namespace DronDonDon.Location.UI.Screen
{
    [UIController("UI/Screen/pfLocationScreen@embeded")]
    public class LocationScreen : MonoBehaviour
    {
        [Inject] 
        private OverlayManager _overlayManager;
        
        [UICreated]
        private void Init()
        {
           
        }

        private void OnDestroy()
        {
            _overlayManager.DestroyGameOverlay();
        }

        private void OnWorldCreated()
        {
           
        }

        private void AddUIScreenLoader()
        {
          
        }
    }
}