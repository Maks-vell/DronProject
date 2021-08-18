using DronDonDon.Location.Model.BaseModel;

namespace DronDonDon.Location.Model.Object
{
    public class ObjectModel : PrefabModel
    {
        public void Awake()
        {
            ObjectType = WorldObjectType.OBSTACLE;
        }
    }
}