using System.Collections.Generic;
using System.Linq;
using AgkCommons.L10n.Service;
using AgkCommons.Resources.DataSource;
using AgkCommons.Resources.ResourcePacks;
using AgkCommons.Sound.DataSource;
using AgkUI.Core.Service;
using DronDonDon.Core.Audio;
using IoC;
using IoC.Api;
using UnityEngine;

namespace DronDonDon.Core.IoC
{
    public class GameKitModule : IIoCModule
    {
        private static readonly Dictionary<long, long> CACHE_MEMORY = new Dictionary<long, long>() {
                {1100, 100 * 1024 * 1024},
                {1600, 150 * 1024 * 1024},
                {long.MaxValue, 200 * 1024 * 1024},
        };

        public void Configure(IIoCContainer container)
        {
            container.RegisterSingleton<TranslationRepository>();
            container.RegisterSingleton<L10nService>();
            container.RegisterSingleton<IResourceDataSource, ResourceDataSource>();
            container.RegisterSingleton<ISoundDataSource, SoundDataSource>();
            container.RegisterSingleton<UIService>();
            container.RegisterSingleton<IResourcePackService>(CreateCachedResourcePackService);
        }

        private IResourcePackService CreateCachedResourcePackService()
        {
            CachedResourcePackService result = AppContext.Container.gameObject.AddComponent<CachedResourcePackService>();
            result.Init(GetCacheSize());
            return result;
        }

        private long GetCacheSize()
        {
            long systemMemorySize = SystemInfo.systemMemorySize * 1024 * 1024;
            List<long> memorySizes = CACHE_MEMORY.Keys.ToList();
            memorySizes.Sort();
            foreach (long memorySize in memorySizes) {
                if (systemMemorySize < memorySize) {
                    return CACHE_MEMORY[memorySize];
                }
            }
            return CACHE_MEMORY[0];
        }
    }
}