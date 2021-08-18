using UnityEngine;

namespace DronDonDon.Location.Model.BaseModel
{
    public class PrefabModel : MonoBehaviour
    {
        [SerializeField]
        private WorldObjectType _objectType;
        public WorldObjectType ObjectType
        {
            get { return _objectType; }
            protected set { _objectType = value; }
        }
    }
}