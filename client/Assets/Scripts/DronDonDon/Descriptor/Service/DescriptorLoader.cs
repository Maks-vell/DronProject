using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Adept.Logger;
using AgkCommons.Resources;
using IoC.Attribute;
using RSG;
using UnityEngine;
using static CsPreconditions.Preconditions;

namespace DronDonDon.Descriptor.Service
{
    public class DescriptorLoader
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<DescriptorLoader>();

        private const string DESCRIPTORS_PATH = "Descriptor/";

        private readonly Dictionary<string, Type> _descriptors = new Dictionary<string, Type>();
        [Inject]
        private ResourceService _resourceService;

        public DescriptorLoader AddDescriptor<T>(string descriptorName)
        {
            _descriptors.Add(descriptorName, typeof(T));
            return this;
        }

        public IPromise Load()
        {
            DescriptorRegistry registry = DescriptorRegistry.Instance;
            return LoadDescriptors(registry);
        }

        private IPromise LoadDescriptors(DescriptorRegistry registry)
        {
            registry.ClearSingleDescriptors();
            List<IPromise> promises = new List<IPromise>();
            foreach (KeyValuePair<string, Type> descriptor in _descriptors) {
                promises.Add(LoadTextAsset(descriptor.Key)
                             .Then((asset => CreateSingleDescriptor(asset, descriptor.Value, registry)))
                             .Catch(e => {
                                 _logger.Error($"Can't parse descriptor {descriptor.Key}", e);
                                 throw e;
                             }));
            }
            return Promise.All(promises);
        }

        private void CreateSingleDescriptor(TextAsset asset, Type descriptorValue, DescriptorRegistry registry)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(asset.text);
            CheckNotNull(doc.DocumentElement);
            XmlNode xmlNode = doc.DocumentElement.FirstChild;

            XmlSerializer xmlSerializer = new XmlSerializer(descriptorValue);
            using (TextReader textReader = new StringReader(xmlNode.OuterXml)) {
                registry.AddSingleDescriptor(xmlSerializer.Deserialize(textReader));
            }
        }

        private IPromise<TextAsset> LoadTextAsset(string name)
        {
            string path = DESCRIPTORS_PATH + name.Substring(0, name.LastIndexOf('.')) + "@embeded";
            IPromise<TextAsset> promise = _resourceService.LoadResource<TextAsset>(path);
            return promise;
        }
    }
}