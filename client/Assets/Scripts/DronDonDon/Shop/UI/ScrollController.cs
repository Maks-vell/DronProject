using System.Collections.Generic;
using AgkUI.Binding.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace DronDonDon.Shop.UI
{
    [UIController("UI/Scrolls/pfScrollViewDroneStore@embeded")]
    public class ScrollController : MonoBehaviour
    {
        [FormerlySerializedAs("Control")] public ListPositionCtrl control;
        [UICreated]
        private void Init(List<ShopItemPanel> shopItemPanels)
        {
            ListPositionCtrl control = gameObject.GetComponent<ListPositionCtrl>();
            this.control = control;
            foreach (var itemPanel in shopItemPanels)
            {
                control.listBoxes.Add(itemPanel.GetComponent<ListBox>());
            }
        }
    }
}