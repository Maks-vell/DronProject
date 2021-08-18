using DronDonDon.Location.World.Dron.Descriptor;

namespace DronDonDon.Location.World.Dron.Model
{
    public class DronViewModel
    {
        private DronDescriptor _dronDescriptor;

        public DronDescriptor DronDescriptor
        {
            get => _dronDescriptor;
            set => _dronDescriptor = value;
        }
    }
}