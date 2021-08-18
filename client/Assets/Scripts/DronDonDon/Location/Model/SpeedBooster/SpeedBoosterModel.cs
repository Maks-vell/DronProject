using DronDonDon.Location.Model.BaseModel;

namespace DronDonDon.Location.Model.SpeedBooster
{
    public class SpeedBoosterModel : PrefabModel
    {
        public float SpeedBoost { get; private set; }
        public float Duration { get; private set; }
        public float NeedsEnergy{ get; private set; }

        public void Awake()
        {
            ObjectType = WorldObjectType.SPEED_BUSTER;
            SpeedBoost = 16f;
            Duration = 5f;
            NeedsEnergy = 3f;
        }
    }
}