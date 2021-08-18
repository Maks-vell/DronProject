using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Dialog.Attributes;
using AgkUI.Dialog.Service;
using DronDonDon.Core.UI.Dialog;
using IoC.Attribute;
using IoC.Util;
using UnityEngine;

namespace DronDonDon.Settings.UI
{
    [UIController("UI/Dialog/pfDownloadDialog@embeded")]
    [UIDialogFog(FogPrefabs.EMBEDED_SHADOW_FOG)]
    public class DownloadedDialog : MonoBehaviour
    {
        [Inject]
        private IoCProvider<DialogManager> _dialogManager;
        
        [UIOnClick("pfBackground")]
        private void CloseButton()
        {
            _dialogManager.Require()
                .Hide(gameObject);
        }
    }
}