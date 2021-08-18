using System.Collections.Generic;
using Adept.Logger;
using IoC;
using UnityEngine;

namespace DronDonDon.Core.Filter
{
    public class AppFilterChain : MonoBehaviour
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<AppFilterChain>();
        
        private readonly Queue<IAppFilter> _filters = new Queue<IAppFilter>();

        public void AddFilter(IAppFilter filter)
        {
            _filters.Enqueue(filter);
        }

        public void Next()
        {
            if (_filters.Count == 0) {
                _logger.Debug($"{GetType().Name}.{nameof(Next)} ALL FILTERS RUNED!");
                Destroy(this);
                return;
            }
            IAppFilter filter = _filters.Dequeue();
            _logger.Debug($"{GetType().Name}.{nameof(Next)} start filter {filter.GetType().Name}");
            if (AppContext.Container != null) {
                AppContext.Inject(filter);
            }

            _logger.Debug($"{GetType().Name}.{nameof(Next)} run filter {filter.GetType().Name}");
            filter.Run(this);
        }
    }
}