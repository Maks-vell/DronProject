using AgkCommons.Event;
using DronDonDon.Inventory.Model;

namespace DronDonDon.Inventory.Event
{
    public class InventoryEvent : GameEvent
    {
        public const string UPDATED = "UpdateInventory";

        public InventoryItemModel Item { get; }
        
        public InventoryItemTypeModel InventoryType { get; }

        public InventoryEvent(string name, InventoryItemModel item, InventoryItemTypeModel inventoryType) : base(name)
        {
            Item = item;
            InventoryType = inventoryType;
        }
    }
}