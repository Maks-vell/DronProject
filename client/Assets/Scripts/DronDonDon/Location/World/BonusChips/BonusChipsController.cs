using DronDonDon.Location.Model;
using DronDonDon.Location.Model.BonusChips;
using UnityEngine;

namespace DronDonDon.Location.World.BonusChips
{
    public class BonusChipsController : MonoBehaviour,  IWorldObjectController<BonusChipsModel>
    {
        public WorldObjectType ObjectType { get; private set; }
        public void Init(BonusChipsModel model)
        {
            ObjectType = model.ObjectType;
        }
    }
}