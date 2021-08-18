using DronDonDon.Location.Model.BaseModel;

namespace DronDonDon.Location.Model.Finish
{
    public class FinishModel : PrefabModel
    {
        public void Awake()
        {
            ObjectType = WorldObjectType.FINISH;
        }
    }
}