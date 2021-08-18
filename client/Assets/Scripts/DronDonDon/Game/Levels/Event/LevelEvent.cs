using AgkCommons.Event;

namespace DronDonDon.Game.Levels.Event
{
    public class LevelEvent  : GameEvent
    {
        public const string UPDATED = "levelUpdated";
        public LevelEvent(string name) : base(name)
        {
        }
    }
}