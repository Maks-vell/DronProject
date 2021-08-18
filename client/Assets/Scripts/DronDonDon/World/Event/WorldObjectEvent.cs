using AgkCommons.Event;
using DronDonDon.Location.Model;
using JetBrains.Annotations;
using UnityEngine;

namespace DronDonDon.World.Event
{
    [PublicAPI]
    public class WorldObjectEvent : GameEvent
    {
        public const string ADDED = "WorldObjectAdded";
        public const string CHANGED = "WorldObjectChanged";
        public const string SELECTED = "WorldObjectSelected";
        public const string ON_COLLISION = "OnCollision";
        public const string UI_UPDATE = "UiUpdate";
        public const string ACTIVATE_BOOST = "ActivateBoost";
        public const string TAKE_BOOST = "TakeBoost";
        public const string START_GAME = "StartGame";
        public const string END_GAME = "EnGame";
        public const string DRON_BOOST_SPEED = "DronBoostSpeed";

        public GameObject _collisionObject;
        public DronStats _dronStats;
        public WorldObjectType _typeBoost;

        public float SpeedBoost;

        public float SpeedBoostTime;

        public WorldObjectEvent(string name, GameObject target) : base(name, target)
        {
            _collisionObject = target;
        }
        
        public WorldObjectEvent(string name, DronStats dronStats) : base(name)
        {
            _dronStats = dronStats;
        }

        public WorldObjectEvent(string name, WorldObjectType type): base(name)
        {
            _typeBoost = type;
        }
        
        public WorldObjectEvent(string name, float speedBoost, float speedBoostTime) : base(name)
        {
            SpeedBoost = speedBoost;
            SpeedBoostTime = speedBoostTime;
        }
        
        public WorldObjectEvent(string name) : base(name)
        {
            
        }

        public T GetController<T>()
        {
            return Target.GetComponent<T>();
        }
    }
}