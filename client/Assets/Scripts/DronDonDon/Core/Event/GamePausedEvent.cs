using AgkCommons.Event;
using JetBrains.Annotations;

namespace DronDonDon.Core.Event
{
    public class GamePausedEvent : GameEvent
    {
        public const string GAME_PAUSED = "gamePausedEvent";

        [PublicAPI]
        public bool PauseStatus { get; }

        public GamePausedEvent(string name, bool pauseStatus) : base(name)
        {
            PauseStatus = pauseStatus;
        }
    }
}