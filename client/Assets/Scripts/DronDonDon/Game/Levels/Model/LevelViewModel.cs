using DronDonDon.Game.Levels.Descriptor;
using DronDonDon.Game.Levels.IoC;

namespace DronDonDon.Game.Levels.Model
{
    public class LevelViewModel
    {
        private LevelProgress _levelProgress;
        private LevelDescriptor _levelDescriptor;

        public LevelProgress LevelProgress
        {
            get => _levelProgress;
            set => _levelProgress = value;
        }
        
        public LevelDescriptor LevelDescriptor
        {
            get => _levelDescriptor;
            set => _levelDescriptor = value;
        }
    }
}