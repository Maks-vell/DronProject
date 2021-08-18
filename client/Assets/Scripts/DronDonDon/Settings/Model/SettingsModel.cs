namespace DronDonDon.Settings.Model
{
    public class SettingsModel
    {
        private bool _isMusicMute;
        private bool _isSoundMute;

        public bool IsSoundMute
        {
            get { return _isSoundMute; }
            set { _isSoundMute = value; }
        }
        public bool IsMusicMute
        {
            get { return _isMusicMute; }
            set { _isMusicMute = value; }
        }
    }
}