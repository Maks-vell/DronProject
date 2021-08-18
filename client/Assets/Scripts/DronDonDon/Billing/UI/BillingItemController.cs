using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Element.Text;
using DronDonDon.Billing.Descriptor;
using DronDonDon.Billing.Service;
using IoC.Attribute;
using UnityEngine;
using UnityEngine.UI;

namespace DronDonDon.Billing.UI
{
    [UIController("UI/Items/pfBillingItem@embeded")]
    public class BillingItemController: MonoBehaviour
    {
        [Inject] 
        private BillingService _billingService;

        [Inject] 
        private BillingDescriptor _billingDescriptor;
        
        [UIObjectBinding("Count")] 
        private GameObject _creditCount;
        
        [UIObjectBinding("Price")] 
        private GameObject _price;
        
        [UIObjectBinding("IconChip")] 
        private GameObject _iconChip;
        
        [UICreated]
        private void Init(BillingDescriptor descriptor)
        {
            SetIcon(descriptor.Icon);
            SetPrice(descriptor.Price);
            SetCredits(descriptor.Credits);
            _billingDescriptor = descriptor;
        }

        private void SetIcon(string iconPath)
        {
            _iconChip.GetComponent<Image>().sprite = Resources.Load(iconPath, typeof(Sprite)) as Sprite;
        }
        private void SetPrice(double itemPrice)
        {
            _price.GetComponent<UILabel>().text ="$"+itemPrice;
        }
        private void SetCredits(int creditsCount)
        {
            _creditCount.GetComponent<UILabel>().text = creditsCount.ToString();
        }

        [UIOnClick("Container")]
        private void BuyCredits()
        {
            _billingService.AddCredits(_billingDescriptor.Credits);
        }
    }
}