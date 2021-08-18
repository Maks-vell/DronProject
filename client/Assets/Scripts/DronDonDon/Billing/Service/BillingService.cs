using AgkCommons.Configurations;
using AgkCommons.Event;
using AgkCommons.Resources;
using AgkUI.Dialog.Service;
using DronDonDon.Billing.Descriptor;
using IoC.Attribute;
using DronDonDon.Billing.Model;
using DronDonDon.Billing.Event;
using DronDonDon.Billing.IoC;
using DronDonDon.Core.Filter;
using DronDonDon.Shop.UI;
using IoC.Util;

namespace DronDonDon.Billing.Service
{
    public class BillingService : GameEventDispatcher, IInitable
    {
        [Inject]
        private BillingRepository _creditShopRepository;
        
        [Inject]
        private ResourceService _resourceService;

        [Inject] 
        private BillingDescriptorRegistry _billingDescriptorRegistry;
        
        [Inject]
        private IoCProvider<DialogManager> _dialogManager;

        [Inject] 
        private BillingDescriptor _billingDescriptor;
        public void Init()
        {
            _resourceService.LoadConfiguration("Configs/Billing@embeded", OnConfigLoaded);
        }

        private void OnConfigLoaded(Configuration config, object[] loadparameters)
        {
            foreach (Configuration temp in config.GetList<Configuration>("billing.billingItem"))
            {
                BillingDescriptor shopItemDescriptor = new BillingDescriptor();
                shopItemDescriptor.Configure(temp);
                _billingDescriptorRegistry.BillingDescriptors.Add(shopItemDescriptor);
            }
        }

        private void UpdateSettings()
        {
            PlayerResourceModel playerResourceModel = RequirePlayerResourceModel();
            SetCreditsCount(playerResourceModel.creditsCount);
        }
        
        public PlayerResourceModel RequirePlayerResourceModel()
        {
            PlayerResourceModel model = _creditShopRepository.Get();
            if (model == null)
            {
                InitCreditsCount();
            }
            return _creditShopRepository.Require();
        }
        
        private bool HasCreditShopModel()
        {
            return (_creditShopRepository.Get() != null);
        }
        private void InitCreditsCount()
        {
            if (!HasCreditShopModel()) {
                PlayerResourceModel playerResourceModel = new PlayerResourceModel();
                playerResourceModel.creditsCount = 0;
                _creditShopRepository.Set(playerResourceModel);
            }
            UpdateSettings();
        }
        public void SetCreditsCount(int count)
        {
            PlayerResourceModel playerResourceModel = RequirePlayerResourceModel();
            playerResourceModel.creditsCount = count;
            _creditShopRepository.Set(playerResourceModel);
            Dispatch(new BillingEvent(BillingEvent.UPDATED));
        }
        public int GetCreditsCount()
        {
            return RequirePlayerResourceModel().creditsCount;
        }

        public void AddCredits(int count)
        {
            SetCreditsCount(GetCreditsCount()+count);
        }

        public void ShowDronStoreDialog()
        {
            _dialogManager.Require().Show<ShopDialog>();
        }
    }
}