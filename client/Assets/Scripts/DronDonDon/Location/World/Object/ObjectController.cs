using DronDonDon.Location.Model;
using DronDonDon.Location.Model.Object;
using UnityEngine;

namespace DronDonDon.Location.World.Object
{
    public class ObjectController : MonoBehaviour,  IWorldObjectController<ObjectModel >
    {
        public WorldObjectType ObjectType { get; private set; }
        public void Init(ObjectModel  model)
        {
            ObjectType = model.ObjectType;
        }
    }
}