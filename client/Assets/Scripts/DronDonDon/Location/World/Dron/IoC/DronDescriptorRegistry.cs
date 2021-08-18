using System.Collections.Generic;
using DronDonDon.Location.World.Dron.Descriptor;

namespace DronDonDon.Location.World.Dron.IoC
{
    public class DronDescriptorRegistry
    {
        private List<DronDescriptor> _dronDescriptors;
        
        public DronDescriptorRegistry()
        {
            _dronDescriptors = new List<DronDescriptor>();
        }
        
        public List<DronDescriptor> DronDescriptors
        {
            get => _dronDescriptors;
            set => _dronDescriptors = value;
        }
        
    }
}