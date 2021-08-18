using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Dialog.Attributes;
using AgkUI.Dialog.Service;
using DronDonDon.Core.UI.Dialog.Service;
using IoC.Attribute;
using IoC.Util;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace DronDonDon.Core.UI.Dialog
{
    [UIController(DialogService.CONFIRM_DIALOG)]
    [UIDialogFog(FogPrefabs.EMBEDED_SHADOW_FOG)]
    public class ConfirmDialog : MonoBehaviour
    {
        public delegate void ConfirmDelegate(ConfirmResult result);

        [UIComponentBinding("TitleText")]
        private TextMeshProUGUI _title;

        [UIComponentBinding("MessageText")]
        private TextMeshProUGUI _message;

        [UIComponentBinding("YesButtonTitle")]
        private TextMeshProUGUI _yesButtonTitle;

        [UIComponentBinding("NoButtonTitle")]
        private TextMeshProUGUI _noButtonTitle;

        [CanBeNull]
        private ConfirmDelegate _callback;

        [Inject]
        private IoCProvider<DialogManager> _dialogProvider;

        [UICreated]
        private void Init(string title, string message, string yesButtonTitle, string noButtonTitle, ConfirmDelegate callback)
        {
            Title = title;
            Message = message;
            YesButtonTitle = yesButtonTitle;
            NoButtonTitle = noButtonTitle;
            _callback = callback;
        }

        [UIOnClick("YesButton")]
        private void OnYesClick()
        {
            _dialogProvider.Require().Hide(this);
            _callback?.Invoke(ConfirmResult.YES);
        }

        [UIOnClick("NoButton")]
        private void OnNoClick()
        {
            _dialogProvider.Require().Hide(this);
            _callback?.Invoke(ConfirmResult.NO);
        }

        private string Title
        {
            set { _title.text = value; }
        }

        private string Message
        {
            set { _message.text = value; }
        }

        private string YesButtonTitle
        {
            set { _yesButtonTitle.text = value; }
        }

        private string NoButtonTitle
        {
            set { _noButtonTitle.text = value; }
        }

        public enum ConfirmResult
        {
            YES,
            NO
        }
    }
}