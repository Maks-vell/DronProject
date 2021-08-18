using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace DronDonDon.Shop.Descriptor
{
    public class ShopDescriptor
    {
        private List<ShopItemDescriptor> _shopItemDescriptors;
        
        public List<ShopItemDescriptor> ShopItemDescriptors
        {
            get => _shopItemDescriptors;
            set => _shopItemDescriptors = value;
        }
        public ShopDescriptor()
        {
            _shopItemDescriptors = new List<ShopItemDescriptor>();
        }

        public ShopItemDescriptor RequireShopItem(string itemId)
        {
            ShopItemDescriptor shopItemDescriptor = GetShopItems().FirstOrDefault(x => x.Id == itemId);
            if (shopItemDescriptor == null) {
                throw new NullReferenceException($"ShopItemDescriptor with id= {itemId} of collection not found");
            }
            return shopItemDescriptor;
        }

        [CanBeNull]
        public ShopItemDescriptor GetShopItem(string itemId)
        {
            try
            {
                return GetShopItems().FirstOrDefault(x => x.Id == itemId);
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
        private List<ShopItemDescriptor> GetShopItems()
        {
            return ShopItemDescriptors;
        }
    }

    
}