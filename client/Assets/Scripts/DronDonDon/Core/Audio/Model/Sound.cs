namespace DronDonDon.Core.Audio.Model
{
    public class Sound
    {
        private readonly string _soundName;
        private readonly string _soundPath;
        private readonly int _track;
        private readonly bool _looped;

        public Sound(string soundName, string soundPath, int track, bool looped)
        {
            _soundName = soundName;
            _soundPath = soundPath;
            _track = track;
            _looped = looped;
        }

        public string SoundName
        {
            get { return _soundName; }
        }
        public string SoundPath
        {
            get { return _soundPath; }
        }
        public int Track
        {
            get { return _track; }
        }
        public bool Looped
        {
            get { return _looped; }
        }
    }
}