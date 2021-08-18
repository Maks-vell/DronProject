using DronDonDon.Location.Service;
using DronDonDon.Location.Service.Builder;
using IoC.Api;

namespace DronDonDon.Location.IoC
{
    public class LocationModule : IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<LocationService>();      
            container.RegisterSingleton<LocationBuilderManager>();
            container.RegisterSingleton<CreateObjectService>();
            container.RegisterSingleton<GameService>();
        }
    }
}