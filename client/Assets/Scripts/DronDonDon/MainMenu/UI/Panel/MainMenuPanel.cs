using System;
using DronDonDon.Shop.UI;
using DronDonDon.Billing.UI;
using DronDonDon.Billing.Event;
using DronDonDon.Billing.Service;
using DronDonDon.Settings.UI;
using Adept.Logger;
using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Core.Model;
using AgkUI.Core.Service;
using AgkUI.Dialog.Service;
using AgkUI.Element.Text;
using DronDonDon.Core;
using DronDonDon.Game.Levels.Service;
using DronDonDon.Game.Levels.UI;
using DronDonDon.Resource.UI.DescriptionLevelDialog;
using IoC.Attribute;
using IoC.Util;
using UnityEngine;
using LocationService = DronDonDon.Location.Service.LocationService;

namespace DronDonDon.MainMenu.UI.Panel
{
    [UIController(PREFAB)]
    public class MainMenuPanel : MonoBehaviour
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<MainMenuPanel>();
        private const string PREFAB = "UI/Panel/pfMainScreenPanel@embeded";
        
        [Inject]
        private IoCProvider<OverlayManager> _overlayManager;     
        
        [Inject] 
        private BillingService _billingService;
        
        [Inject]
        private UIService _uiService;

        [Inject] 
        private IoCProvider<DialogManager> _dialogManager;
        
        [UIObjectBinding("MiddlePanel")]
        private GameObject _middlePanel;
        
        [UIComponentBinding("CountChips")]
        private UILabel _countChips;

        public void Init()
        {
            _overlayManager.Require().HideLoadingOverlay(true);
            _uiService.Create<ProgressMapController>(UiModel.Create<ProgressMapController>().Container(_middlePanel)).Done();
            _billingService.AddListener<BillingEvent>(BillingEvent.UPDATED, OnResourceUpdated);
            UpdateCredits();
            _logger.Debug("MainMenuPanel start init");
        }

        private void OnDestroy()
        {
         _billingService.RemoveListener<BillingEvent>(BillingEvent.UPDATED, OnResourceUpdated);   
        }

        private void UpdateCredits()
        {
            _countChips.text = _billingService.GetCreditsCount().ToString();
        }
        
        private void OnResourceUpdated(BillingEvent resourceEvent)
        {
            UpdateCredits();
        }

        [UIOnClick("DronShop")]
        private void OnDroneStore()
        {
            _dialogManager.Require().Show<ShopDialog>();
            _logger.Debug("Click on store");
        }
        
        [UIOnClick("SettingsButton")]
        private void OnSettingsPanel()
        {
            _dialogManager.Require().Show<GameSettingsDialog>();
            _logger.Debug("Click on settings");
        }
        
        [UIOnClick("StoreChipsButton")]
        private void OnCreditsPanel()
        {
            _logger.Debug("Click on credits");
            _dialogManager.Require().Show<CreditShopDialog>();
        }
    }
}