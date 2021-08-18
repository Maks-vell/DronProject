using Adept.Logger;
using AgkCommons.IoC;
using DronDonDon.Console.IoC;
using DronDonDon.Core.Audio.IoC;
using DronDonDon.Core.IoC;
using DronDonDon.Core.UI.IoC;
using DronDonDon.Descriptor.IoC;
using DronDonDon.Game.Levels.IoC;
using DronDonDon.Location.IoC;
using DronDonDon.Settings.IoC;
using DronDonDon.Billing.IoC;
using DronDonDon.Inventory.IoC;
using DronDonDon.Shop.IoC;
using DronDonDon.Location.World.Dron.Module;
using IoC;
using IoC.Api;
using IoC.Extension;

namespace DronDonDon.Core.Filter
{
    public class IoCFilter : IAppFilter
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<IoCFilter>();

        public void Run(AppFilterChain chain)
        {
            _logger.Info("Start IoC initialization");
            IoCContainer container = chain.gameObject.AddComponent<IoCContainer>();
            MonoBehaviourExtension.SetDependencyInjector(container, container);
            AppContext.Container = container;
            Configure(container);

            chain.Next();
        }

        private void Configure(IIoCContainer context)
        {
            context.RegisterModule(new AgkCommonsModule());
            context.RegisterModule(new GameKitModule());     
            context.RegisterModule(new ConsoleModule());
            context.RegisterModule(new AudioModule());
            context.RegisterModule(new UIModule());
            context.RegisterModule(new CoreModule());
            context.RegisterModule(new DescriptorModule());    
            context.RegisterModule(new LocationModule());
            context.RegisterModule(new SettingsModule());
            context.RegisterModule(new LevelsModule());
            context.RegisterModule(new BillingModule());
            context.RegisterModule(new ShopModule());
            context.RegisterModule(new InventoryModule());
            context.RegisterModule(new DronModule());
        }
    }
}