using System;
using Adept.Logger;
using AgkUI.Dialog.Service;
using IoC.Attribute;
using IoC.Extension;
using IoC.Util;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable UnusedMember.Local

namespace DronDonDon.Core.UI.Dialog.Service
{
    public class DialogService : MonoBehaviour
    {
        private readonly IAdeptLogger _logger = LoggerFactory.GetLogger<DialogService>();
        public const string ALERT_DIALOG = "UI/Dialog/Standart/pfAlertDialog@embeded";
        public const string FAKE_AD_DIALOG = "UI/Dialog/Standart/pfFakeAdDialog@embeded";
        public const string CONFIRM_DIALOG = "UI/Dialog/Standart/pfConfirmDialog@embeded";
        public const string ERROR_DIALOG = "UI/Dialog/Standart/pfErrorDialog@embeded";
        public const string UNHANDLED_ERROR_DIALOG = "UI/Dialog/Standart/pfUnhandledErrorDialog@embeded";

        private const string DEFAULT_ERROR_TITLE = "Ошибка";
        private const string DEFAULT_ERROR_MESSAGE = "Произошла ошибка";
        private const string YES_BUTTON_TITLE = "Yes";
        private const string NO_BUTTON_TITLE = "No";

        [Inject]
        private IoCProvider<DialogManager> _dialogManagerProvider;

        private void Awake()
        {
            this.InjectAll();
        }

        public void ShowAlert(string title, string message, [CanBeNull] Action callback = null)
        {
            DialogManager dialogManager = _dialogManagerProvider.Get();
            if (dialogManager == null) {
                return;
            }
            dialogManager.ShowModal<AlertDialog>(title, message, callback);
        }

        [PublicAPI]
        public void ShowSystemError([CanBeNull] string message, [CanBeNull] string errorLog = null, [CanBeNull] UnityAction callback = null)
        {
            DialogManager dialogManager = _dialogManagerProvider.Get();
            if (dialogManager == null) {
                return;
            }
            errorLog = errorLog ?? Environment.StackTrace;
            dialogManager.ShowModal<ErrorDialog>(DEFAULT_ERROR_TITLE, message ?? DEFAULT_ERROR_MESSAGE, callback, errorLog);
        }

        [PublicAPI]
        public void ShowDefaultError([CanBeNull] string message = null, [CanBeNull] UnityAction callback = null)
        {
            ShowSystemError(message, null, callback);
        }
        

        [PublicAPI]
        public void Confirm(string title, string message, ConfirmDialog.ConfirmDelegate callback)
        {
            Confirm(title, message, YES_BUTTON_TITLE, NO_BUTTON_TITLE, callback);
        }

        [PublicAPI]
        public void Confirm(string title, string message, string yesButtonTitle, string noButtonTitle, ConfirmDialog.ConfirmDelegate callback)
        {
            DialogManager dialogManager = _dialogManagerProvider.Get();
            if (dialogManager == null) {
                return;
            }
            dialogManager.ShowModal<ConfirmDialog>(title, message, yesButtonTitle, noButtonTitle, callback);
        }
    }
}