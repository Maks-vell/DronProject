using DronDonDon.Inventory.Service;
using IoC.Api;

namespace DronDonDon.Inventory.IoC
{
    public class InventoryModule : IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<InventoryService>();
            container.RegisterSingleton<InventoryRepository>();
        }
    }
}