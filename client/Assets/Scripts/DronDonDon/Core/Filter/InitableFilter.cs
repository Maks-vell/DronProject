using System.Collections.Generic;
using Adept.Logger;
using IoC;
using RSG;

namespace DronDonDon.Core.Filter
{
    public class InitableFilter : IAppFilter
    {
        private IAdeptLogger _logger = LoggerFactory.GetLogger<InitableFilter>();

        public void Run(AppFilterChain chain)
        {
            List<IPromise> promises = new List<IPromise>();
            foreach (IInitable serviceInitable in AppContext.ResolveCollection<IInitable>())
            {
                serviceInitable.Init();
            }
        }
    }
}