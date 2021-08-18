using DronDonDon.Shop.Descriptor;
using DronDonDon.Shop.Service;
using IoC.Api;

namespace DronDonDon.Shop.IoC
{
    public class ShopModule : IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<ShopService>();
            container.RegisterSingleton<ShopDescriptor>();
        }
    }
}