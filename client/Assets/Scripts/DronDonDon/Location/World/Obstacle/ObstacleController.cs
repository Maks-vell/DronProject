using DronDonDon.Location.Model;
using DronDonDon.Location.Model.Obstacle;
using UnityEngine;

namespace DronDonDon.Location.World.Obstacle
{
    public class ObstacleController : MonoBehaviour,  IWorldObjectController<ObstacleModel>
    {
        public WorldObjectType ObjectType { get; private set; }

        public void Init(ObstacleModel model)
        {
            ObjectType = model.ObjectType;
        }
    }
}