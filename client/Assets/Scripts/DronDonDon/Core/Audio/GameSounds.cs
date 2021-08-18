using DronDonDon.Core.Audio.Model;

namespace DronDonDon.Core.Audio
{
    public static class GameSounds
        {
            public static readonly Sound BOOSTER_PICKUP =
                new Sound("sndBoosterPickup", "Audio/Sound/sndBoosterPickup@embeded", 1, false);
        
            public static readonly Sound SPEED_ACTIVATED =
                new Sound("sndSpeedActivated", "Audio/Sound/sndSpeedActivated@embeded", 2, false);
        
            public static readonly Sound SHIELD_ACTIVATED =
                new Sound("sndShieldActivated", "Audio/Sound/sndShieldActivated@embeded", 3, false);

            public static readonly Sound CHIP_PICKUP =
                new Sound("sndChipPickup", "Audio/Sound/sndChipPickup@embeded", 4, false);

            public static readonly Sound COLLISION =
                new Sound("sndCollision", "Audio/Sound/sndCollision@embeded", 5, false);

            public static readonly Sound DRON_CRASHED = 
                new Sound("sndCrashed", "Audio/Sound/sndCrashed@embeded", 6, false);

            public static readonly Sound DRON_LANDING =
                new Sound("sndDronLanding", "Audio/Sound/sndDronLanding@embeded", 7, false);

            public static readonly Sound DRON_TAKEOFF =
                new Sound("sndDronTakeoff", "Audio/Sound/sndDronTakeoff@embeded", 8, false);

            public static readonly Sound SHOW_DIALOG =
                new Sound("sndShowDialog", "Audio/Sound/sndShowDialog@embeded", 9, false);
        }
    
}