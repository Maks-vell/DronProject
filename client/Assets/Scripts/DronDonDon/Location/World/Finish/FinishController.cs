using DronDonDon.Location.Model;
using DronDonDon.Location.Model.Finish;
using UnityEngine;

namespace DronDonDon.Location.World.Finish
{
    public class FinishController : MonoBehaviour,  IWorldObjectController<FinishModel>
    {
        public WorldObjectType ObjectType { get; private set; }
        public void Init(FinishModel  model)
        {
            ObjectType = model.ObjectType;
        }
    }
}