using System.Collections.Generic;
using AgkCommons.Resources;
using DronDonDon.World;
using RSG;
using UnityEngine;
using DronDonDon.Location.Model.BaseModel;
using Adept.Logger;


namespace DronDonDon.Location.Service.Builder
{
    public class LocationBuilder
    {
        private const string WORLD_NAME = "location";

        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<LocationBuilder>();
        private readonly CreateObjectService _createCreateService;
        private readonly ResourceService _resourceService;
        
        private Promise _promise;
        private string _prefab;
        private Transform _container;

        private LocationBuilder(ResourceService resourceService, CreateObjectService createService)
        {
            _resourceService = resourceService;
            _createCreateService = createService;
        }

        public static LocationBuilder Create(ResourceService resourceService, CreateObjectService createService)
        {
            return new LocationBuilder(resourceService, createService);
        }

        public LocationBuilder Prefab(string prefab)
        {
            _prefab = prefab;
            return this;
        }

        public LocationBuilder Container(Transform container)
        {
            _container = container;
            return this;
        }

        public IPromise Build()
        {
            _promise = new Promise();
            _resourceService.LoadPrefab(_prefab, OnPrefabLoad);
            return _promise;
        }

        private void OnPrefabLoad(GameObject loadedObject, object[] loadparameters)
        {
            loadedObject = Object.Instantiate(loadedObject, _container);
            
            GameWorld gameWorld = loadedObject.AddComponent<GameWorld>();
            gameWorld.CreateWorld(WORLD_NAME);
            
            InitControllers(gameWorld);
            InitService();
            
            _promise.Resolve();
        }

        private static void InitService()
        {
        }

        private void InitControllers(GameWorld gameWorld)
        {
            List<PrefabModel> objectComponents = gameWorld.GetObjectComponents<PrefabModel>();
            foreach (PrefabModel prefabModel in objectComponents) {
                _logger.Debug("attach");
                _createCreateService.AttachController(prefabModel);
            }
        }
    }
}