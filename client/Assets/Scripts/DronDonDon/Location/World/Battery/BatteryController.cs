using DronDonDon.Location.Model;
using DronDonDon.Location.Model.Battery;
using UnityEngine;

namespace DronDonDon.Location.World.Battery
{
    public class BatteryController : MonoBehaviour,  IWorldObjectController<BatteryModel>
    {
        public WorldObjectType ObjectType { get; private set; }
        public void Init(BatteryModel model)
        {
            ObjectType = model.ObjectType;
        }
    }
}