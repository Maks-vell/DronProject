using System.Collections.Generic;
using AgkCommons.Configurations;
using JetBrains.Annotations;

namespace DronDonDon.Core.Configurations
{
    public static class Config
    {
        private static Configuration Instance { get; set; }
        public static List<string> Languages { get; } = new List<string>();
        public static void Init(Configuration configuration)
        {
            Instance = configuration;
            ParseLanguages();
        }
        
        public static DeviceType DeviceType
        {
            get
            {
#if UNITY_EDITOR || PLATFORM_STANDALONE
                return DeviceType.WINDOWS;
#elif UNITY_IOS
                return DeviceType.IOS;
#elif UNITY_ANDROID
                return DeviceType.ANDROID;
#elif UNITY_STANDALONE
                return return DeviceType.WINDOWS;
#else
                throw new NotImplementedException("Unsupported platform");
#endif
            }
        }
        [UsedImplicitly]
        public static string WindowsAppId
        {
            get { return Instance.GetString("windowsAppId", ""); }
        }
        
        [UsedImplicitly]
        public static string Version
        {
            get { return Instance.GetString("version", string.Empty); }
        }
        
        [UsedImplicitly]
        public static bool Production
        {
            get { return Instance.GetBoolean("production"); }
        }  
        [UsedImplicitly]
        public static bool ShowConsole
        {
            get { return Instance.GetBoolean("showConsole", true); }
        }
        
        public static string ApiUrl
        {
            get { return Instance.GetString("apiUrl", "http://localhost:9999"); }
        }

        private static void ParseLanguages()
        {
            Languages.Clear();
            Configuration languagesConfig = Instance.GetConfiguration("languages");
            if (languagesConfig == null) {
                return;
            }
            foreach (object languageConfigObject in languagesConfig.GetList("lang")) {
                Configuration languageConfig = (Configuration) languageConfigObject;
                if (languageConfig == null) {
                    continue;
                }
                Languages.Add(languageConfig.GetString("id"));
            }
        }
    }
}