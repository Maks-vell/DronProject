using System;
using AgkCommons.Configurations;
using AgkCommons.Event;
using AgkCommons.Resources;
using AgkUI.Dialog.Service;
using DronDonDon.Billing.Model;
using DronDonDon.Billing.Service;
using DronDonDon.Billing.UI;
using DronDonDon.Core.Filter;
using DronDonDon.Inventory.Model;
using DronDonDon.Inventory.Service;
using IoC.Attribute;
using DronDonDon.Shop.Descriptor;
using DronDonDon.Shop.Event;
using DronDonDon.Shop.UI;
using IoC.Util;

namespace DronDonDon.Shop.Service
{
    public class ShopService : GameEventDispatcher, IInitable
    {
        [Inject]
        private ResourceService _resourceService;
        
        [Inject]
        private ShopDescriptor _shopDescriptor;

        [Inject]
        private InventoryService _inventoryService;

        [Inject] 
        private BillingService _billingService;
        [Inject] 
        private PlayerResourceModel _resourceModel;
        
        [Inject]
        private IoCProvider<DialogManager> _dialogManager;

        public void Init()
        {
            _resourceService.LoadConfiguration("Configs/shop@embeded", OnConfigLoaded);
        }

        public void RemoveListener()
        {
            Dispatch(new ShopEvent(ShopEvent.CLOSE_DIALOG));
        }

        public bool BuyDron(string itemId)
        {
            _resourceModel = _billingService.RequirePlayerResourceModel();
            ShopItemDescriptor shopItemDescriptor = _shopDescriptor.GetShopItem(itemId);
            if (shopItemDescriptor == null) {
                throw new Exception("ShopItem not found, itemId = " + itemId);
            }
            if (_resourceModel.creditsCount >= shopItemDescriptor.Price)
            {
                _billingService.SetCreditsCount(_resourceModel.creditsCount - shopItemDescriptor.Price);
                InventoryItemModel item = new InventoryItemModel(itemId, shopItemDescriptor.Type, 1);
                _inventoryService.AddInventory(item);
                return true;
            }
            else
            {
                _dialogManager.Require().ShowModal<BuyDialog>(false);
                return false;
            }
        }
        private void OnConfigLoaded(Configuration config, object[] loadparameters)
        {
            foreach (Configuration temp in config.GetList<Configuration>("shop.shopItem"))
            {
                ShopItemDescriptor shopItemDescriptor = new ShopItemDescriptor();
                shopItemDescriptor.Configure(temp);
                _shopDescriptor.ShopItemDescriptors.Add(shopItemDescriptor);
            }
        }
    }
}