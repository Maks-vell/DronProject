using DronDonDon.Core.Repository;
using DronDonDon.Settings.Model;

namespace DronDonDon.Settings.Service
{
    public class SettingsRepository : LocalPrefsSingleRepository<SettingsModel>
    {
        public SettingsRepository() : base("settingsRepository")    
        {
            
        }
    }
}