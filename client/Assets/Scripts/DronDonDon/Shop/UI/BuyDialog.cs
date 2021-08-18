using System.Collections.Generic;
using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Dialog.Attributes;
using AgkUI.Dialog.Service;
using DronDonDon.Core.UI.Dialog;
using IoC.Attribute;
using IoC.Util;
using Adept.Logger;
using AgkCommons.Event;
using AgkCommons.Input.Gesture.Model;
using AgkCommons.Input.Gesture.Model.Gestures;
using AgkCommons.Input.Gesture.Service;
using AgkUI.Core.Model;
using AgkUI.Core.Service;
using AgkUI.Element.Text;
using DronDonDon.Game.Levels.Event;
using DronDonDon.Inventory.Service;
using DronDonDon.Shop.Descriptor;
using DronDonDon.Shop.Event;
using DronDonDon.Shop.Service;
using UnityEngine;
using DronDonDon.Billing.Service;
using UnityEngine.UI;
using DronDonDon.Billing.Event;
using DronDonDon.Billing.UI;

namespace DronDonDon.Shop.UI
{

    [UIController("UI/Dialog/pfBuyDialog@embeded")]
    [UIDialogFog(FogPrefabs.EMBEDED_SHADOW_FOG)]
    public class BuyDialog: MonoBehaviour
  {
      [Inject]
      private IoCProvider<DialogManager> _dialogManager;

      [Inject] 
      private ShopService _shopService;

      [UIObjectBinding("BuyMessage")] 
      private GameObject _buyMessage;
        
      [UIObjectBinding("BuyDescription")] 
      private GameObject _buyDescription;
        
      [UIObjectBinding("ButtonText")] 
      private GameObject _buttonText;
        
      private bool _trasaction;
        
      [UICreated]
      public void Init(bool transaction)
      {
          _trasaction = transaction;
          SetPropertyPanel(_trasaction);
      }
      [UIOnClick("pfBackground")]
      private void AnyTouch()
      {
          _dialogManager.Require()
              .Hide(gameObject);
      }

      private void SetPropertyPanel(bool condition)
      {
          if (!condition)
          {
              _buyMessage.GetComponent<UILabel>().text = "Упсс...";
              _buyDescription.GetComponent<UILabel>().text = "У вас недостаточно чипов для покупки";
              _buttonText.GetComponent<UILabel>().text = "Магазин чипов";
          }
      }

      [UIOnClick("Button")]
      private void ClickOnButton()
      {
          _dialogManager.Require()
              .Hide(gameObject);
          if(!_trasaction)
          {
              _shopService.RemoveListener();
              _dialogManager.Require()
                  .Hide(GameObject.Find("pfShopDialog(Clone)"));
              _dialogManager.Require().ShowModal<CreditShopDialog>();
          }
            
      }
    }
}