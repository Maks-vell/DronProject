using DronDonDon.Core.Repository;
using DronDonDon.Location.World.Dron.Model;

namespace DronDonDon.Location.World.Dron.Service
{
    public class DronRepository : LocalPrefsSingleRepository<DronModel>
    {
        public DronRepository() : base("dronRepository")    
        {
            
        }
    }
}