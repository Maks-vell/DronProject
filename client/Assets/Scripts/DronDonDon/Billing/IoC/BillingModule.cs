using DronDonDon.Billing.Descriptor;
using IoC.Api;
using DronDonDon.Billing.Service;
using DronDonDon.Billing.Event;
using DronDonDon.Billing.Model;

namespace DronDonDon.Billing.IoC
{
    public class BillingModule: IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<BillingService>();
            container.RegisterSingleton<BillingRepository>();
            container.RegisterSingleton<BillingEvent>();
            container.RegisterSingleton<PlayerResourceModel>();
            container.RegisterSingleton<BillingDescriptorRegistry>();
            container.RegisterSingleton<BillingDescriptor>();
        } 
    }
}