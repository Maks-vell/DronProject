using AgkCommons.Repository;
using DronDonDon.Inventory.Model;

namespace DronDonDon.Inventory.Service
{
    public class InventoryRepository : LocalPrefsSingleRepository<InventoryModel>
    {
        public InventoryRepository() : base("inventoryRepository")    
        {
            
        }
    }
}