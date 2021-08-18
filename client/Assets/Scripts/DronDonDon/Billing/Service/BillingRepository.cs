using DronDonDon.Billing.Model;
using DronDonDon.Core.Repository;

namespace DronDonDon.Billing.Service
{
    public class BillingRepository: LocalPrefsSingleRepository<PlayerResourceModel>
    {
        public BillingRepository() : base("creditShopRepository")    
        {
            
        }
    }
}