using AgkCommons.Event;

namespace DronDonDon.Core.Event
{
    public class GameQuitEvent : GameEvent
    {
        public const string BEFORE_QUIT = "beforeQuitEvent";
        public GameQuitEvent(string name) : base(name)
        {
            
        }
    }
}