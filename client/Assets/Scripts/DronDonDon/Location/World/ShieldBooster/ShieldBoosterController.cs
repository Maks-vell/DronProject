using DronDonDon.Location.Model;
using DronDonDon.Location.Model.ShieldBooster;
using UnityEngine;

namespace DronDonDon.Location.World.ShieldBooster
{
    public class ShieldBoosterController : MonoBehaviour,  IWorldObjectController<ShieldBoosterModel >
    {
        public WorldObjectType ObjectType { get; private set; }
        public void Init(ShieldBoosterModel  model)
        {
            ObjectType = model.ObjectType;
        }
    }
}