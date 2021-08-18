using DronDonDon.Settings.Service;
using IoC.Api;

namespace DronDonDon.Settings.IoC
{
    public class SettingsModule: IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<SettingsService>();
            container.RegisterSingleton<SettingsRepository>();
        }
    }
}