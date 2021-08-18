using DronDonDon.Game.Levels.Repository;
using DronDonDon.Game.Levels.Service;
using IoC.Api;

namespace DronDonDon.Game.Levels.IoC
{
    public class LevelsModule : IIoCModule
    {
        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<ProgressRepository>();
            container.RegisterSingleton<LevelDescriptorRegistry>();
            container.RegisterSingleton<LevelService>();
        }
    }
}