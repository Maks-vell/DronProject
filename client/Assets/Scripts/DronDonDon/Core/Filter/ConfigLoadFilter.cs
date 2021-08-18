using System;
using System.Xml;
using Adept.Logger;
using AgkCommons.Configurations;
using DronDonDon.Core.Configurations;
using JetBrains.Annotations;
using UnityEngine;

namespace DronDonDon.Core.Filter
{
    public class ConfigLoadFilter : IAppFilter
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<ConfigLoadFilter>();
        private const string LOAD_CONFIG_PATH = "Configs/LocalConfig";

        public void Run(AppFilterChain chain)
        {
            Configuration configuration = LoadLocalConfig();
            if (configuration == null) {
                Debug.LogError("Error load local config, restart app");
                GameApplication.Instance.Restart();
                return;
            }
            chain.Next();
        }

        [CanBeNull]
        private Configuration LoadLocalConfig()
        {
            Configuration configuration = new Configuration();
            try {
                TextAsset localConfigData = Resources.Load<TextAsset>(LOAD_CONFIG_PATH);
                if (localConfigData == null) {
                    Debug.LogError("Not found local config! Path=" + LOAD_CONFIG_PATH);
                    return null;
                }

                XmlDocument localConfigXml = new XmlDocument();
                localConfigXml.LoadXml(localConfigData.text);

                Debug.Log("Will load local configuration");

                configuration.LoadXml(localConfigXml);
                Config.Init(configuration);
                ConfigureLogger(localConfigXml);
                _logger.Info("Logger configurated");
            } catch (Exception e) {
                Debug.LogError("Load local config exception: " + e.Message);
                return null;
            }
            return configuration;
        }
        private void ConfigureLogger(XmlDocument xml)
        {
            if (!LoggerConfigurator.Configure(xml, LoggerType.NLOG)) {
                Debug.LogWarning("logger config empty");
            }
        }
    }
}