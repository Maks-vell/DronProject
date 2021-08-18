using AgkCommons.Configurations;

namespace DronDonDon.Location.World.Dron.Descriptor
{
    public class DronDescriptor
    {
        private string _id;
        private string _title;
        private int _energy;
        private int _durability;
        private int _mobility;
        private string _prefab;
        public void Configure(Configuration config)
        {
            Id = config.GetString("id");
            Title = config.GetString("title");
            Energy = config.GetInt("energy");
            Durability = config.GetInt("durability");
            Mobility = config.GetInt("mobility");
            Prefab = config.GetString("prefab");
        }

        public string Id
        {
            get => _id;
            set => _id = value;
        }
        
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public int Energy
        {
            get => _energy;
            private set => _energy = value;
        }

        public int Durability
        {
            get => _durability;
            private set => _durability = value;
        }

        public int Mobility
        {
            get => _mobility;
            private set => _mobility = value;
        }
        
        public string Prefab
        {
            get => _prefab;
            private set => _prefab = value;
        }
    }
}