using DronDonDon.Location.Model;

namespace DronDonDon.Location.World
{
    public interface IWorldObject
    {
        WorldObjectType ObjectType { get; }
    }
}