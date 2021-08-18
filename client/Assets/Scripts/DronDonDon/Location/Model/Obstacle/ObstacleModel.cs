using DronDonDon.Location.Model.BaseModel;

namespace DronDonDon.Location.Model.Obstacle
{
    public class ObstacleModel : PrefabModel
    {
        public float Damage { get; private set; }
        public void Awake()
        {
            ObjectType = WorldObjectType.OBSTACLE;
            Damage = 13f;
        }
    }
}