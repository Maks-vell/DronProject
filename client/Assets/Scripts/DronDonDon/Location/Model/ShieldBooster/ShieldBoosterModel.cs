using DronDonDon.Location.Model.BaseModel;

namespace DronDonDon.Location.Model.ShieldBooster
{
    public class ShieldBoosterModel : PrefabModel
    {
        public float Duration { get; private set; }
        public void Awake()
        {
            ObjectType = WorldObjectType.SHIELD_BUSTER;
            Duration = 5f;
        }
    }
}