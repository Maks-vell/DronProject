using System.Collections.Generic;
using DronDonDon.Billing.Descriptor;

namespace DronDonDon.Billing.IoC
{
    public class BillingDescriptorRegistry
    {
        private List<BillingDescriptor> _billingDescriptors;
        
        public List<BillingDescriptor> BillingDescriptors
        {
            get => _billingDescriptors;
            set => _billingDescriptors = value;
        }
        public BillingDescriptorRegistry()
        {
            _billingDescriptors = new List<BillingDescriptor>();
        }
    }
}