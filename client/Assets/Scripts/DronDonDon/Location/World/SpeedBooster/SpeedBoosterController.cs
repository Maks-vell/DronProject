using DronDonDon.Location.Model;
using DronDonDon.Location.Model.SpeedBooster;
using UnityEngine;

namespace DronDonDon.Location.World.SpeedBooster
{
    public class SpeedBoosterController : MonoBehaviour,  IWorldObjectController<SpeedBoosterModel>
    {
        public WorldObjectType ObjectType { get; private set; }
        public void Init(SpeedBoosterModel  model)
        {
            ObjectType = model.ObjectType;
        }
    }
}