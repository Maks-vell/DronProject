using DronDonDon.Location.Model.BaseModel;
using JetBrains.Annotations;

namespace DronDonDon.Location.World
{
    public interface IWorldObjectController<in T>: IWorldObject
        where T : PrefabModel
    {
        [UsedImplicitly]
            void Init(T model);
    }
}