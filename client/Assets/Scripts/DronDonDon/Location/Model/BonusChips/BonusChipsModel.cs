using DronDonDon.Location.Model.BaseModel;

namespace DronDonDon.Location.Model.BonusChips
{
    public class BonusChipsModel : PrefabModel
    {
        public void Awake()
        {
            ObjectType = WorldObjectType.BONUS_CHIPS;
        }
    }
}