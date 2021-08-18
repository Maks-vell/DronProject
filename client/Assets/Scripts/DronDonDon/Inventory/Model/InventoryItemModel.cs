namespace DronDonDon.Inventory.Model
{
    public class InventoryItemModel
    {
        public string Id { get; set; }
        public InventoryItemTypeModel Type { get; set; }
        
        public int Count { get; set; }
        
        public InventoryItemModel(string itemId, InventoryItemTypeModel type, int count)
        {
            Id = itemId;
            Type = type;
            Count = count;
        }

        public InventoryItemModel()
        {
        }
    }
}