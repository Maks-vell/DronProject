using System.Collections.Generic;
using DronDonDon.Game.Levels.Descriptor;
using NUnit.Framework;

namespace DronDonDon.Game.Levels.IoC
{
    public class LevelDescriptorRegistry
    {
        private List<LevelDescriptor> _levelDescriptors;
        
        public List<LevelDescriptor> LevelDescriptors
        {
            get => _levelDescriptors;
            set => _levelDescriptors = value;
        }

        public LevelDescriptorRegistry()
        {
            _levelDescriptors = new List<LevelDescriptor>();
        }
    }
}