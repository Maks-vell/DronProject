namespace DronDonDon.Location.World.Dron
{
    public class Dron
    {
        private string _id;
        private string _title;
        private int _energy;
        private int _durability;
        private int _mobility;
        private string _prefab;

        public Dron(string id, string title, int energy, int durability, int mobility, string prefab)
        {
            _id = id;
            _title = title;
            _energy = energy;
            _durability = durability;
            _mobility = mobility;
            _prefab = prefab;
        }
    }
}