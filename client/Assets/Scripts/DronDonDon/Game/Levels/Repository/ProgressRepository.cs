using DronDonDon.Core.Repository;
using DronDonDon.Game.Levels.Model;

namespace DronDonDon.Game.Levels.Repository
{
    public class ProgressRepository  : LocalPrefsSingleRepository<PlayerProgressModel>
    {
        public ProgressRepository() : base("progressRepository")    
        {
        }
    }
}