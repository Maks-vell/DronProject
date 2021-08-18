using System;
using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Dialog.Attributes;
using AgkUI.Dialog.Service;
using DronDonDon.Core.UI.Dialog.Service;
using IoC.Attribute;
using IoC.Util;
using TMPro;
using UnityEngine;

namespace DronDonDon.Core.UI.Dialog
{
    [UIController(DialogService.ALERT_DIALOG)]
    [UIDialogFog(FogPrefabs.EMBEDED_SHADOW_FOG)]
    public class AlertDialog : MonoBehaviour
    {
        [UIComponentBinding("TitleText")]
        private TextMeshProUGUI _title;

        [UIComponentBinding("MessageText")]
        private TextMeshProUGUI _message;
        [Inject]
        private IoCProvider<DialogManager> _dialogProvider;

        private Action _callback;

        [UICreated]
        private void Init(string title, string message, Action callback)
        {
            Title = title;
            Message = message;

            _callback = callback;
        }

        [UIOnClick("OkButton")]
        private void OnOkButtonClick()
        {
            _dialogProvider.Require().Hide(this);
            _callback?.Invoke();
        }

        private string Title
        {
            set { _title.text = value; }
        }

        private string Message
        {
            set { _message.text = value; }
        }
    }
}