using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using System;

namespace DronDonDon.Inventory.Model
{
    [UsedImplicitly]
    public class InventoryModel
    {
        public List<InventoryItemModel> Items { get; set; }
        
   
        [CanBeNull]
        public InventoryItemModel GetItem(string itemId)
        {
            try
            {
                return Items.FirstOrDefault(x => x.Id == itemId);
            }
            catch(Exception e)
            {
                return null;
            }
        }
        
        public bool HasItem(string itemId)
        {
            InventoryItemModel item = Items.FirstOrDefault(x => x.Id == itemId);
            return item != null;
        }
    }
}