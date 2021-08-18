using System.Collections.Generic;
using AgkUI.Binding.Attributes;
using UnityEngine;

namespace DronDonDon.Billing.UI
{
    [UIController("UI/Scrolls/pfScrollViewBilling@embeded")]
    public class BillingScrollController : MonoBehaviour
    {
        public ListPositionCtrl Control;
        [UICreated]
        private void Init(List<BillingItemController> billingItemControllers)
        {
            ListPositionCtrl control = gameObject.GetComponent<ListPositionCtrl>();
            Control = control;
            foreach (var itemPanel in billingItemControllers)
            {
                control.listBoxes.Add(itemPanel.GetComponent<ListBox>());
            }
        }
        
        
    }
}