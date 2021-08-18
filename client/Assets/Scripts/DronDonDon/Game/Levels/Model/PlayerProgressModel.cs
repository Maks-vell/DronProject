using System.Collections.Generic;

namespace DronDonDon.Game.Levels.Model
{
    public class PlayerProgressModel
    {
        private List<LevelProgress> _levelsProgress = new List<LevelProgress>();

        public List<LevelProgress> LevelsProgress
        {
            get { return _levelsProgress; }
            set { _levelsProgress = value; }
        }
        
        public PlayerProgressModel()
        {
        }
    }
}