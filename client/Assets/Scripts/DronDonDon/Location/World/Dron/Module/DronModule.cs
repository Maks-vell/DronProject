using DronDonDon.Location.World.Dron.IoC;
using DronDonDon.Location.World.Dron.Service;
using IoC.Api;

namespace DronDonDon.Location.World.Dron.Module
{
    public class DronModule : IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<DronService>();
            container.RegisterSingleton<DronRepository>();
            container.RegisterSingleton<DronDescriptorRegistry>();
        }
    }
}