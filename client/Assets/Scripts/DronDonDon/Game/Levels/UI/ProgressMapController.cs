using System.Collections.Generic;
using Adept.Logger;
using AgkUI.Binding.Attributes;
using AgkUI.Core.Service;
using DronDonDon.Game.Levels.Model;
using DronDonDon.Game.Levels.Service;
using IoC.Attribute;
using UnityEngine;
using AgkUI.Core.Model;
using DronDonDon.Game.Levels.Descriptor;
using DronDonDon.Game.Levels.Event;
using DronDonDon.MainMenu.UI.Panel;

namespace DronDonDon.Game.Levels.UI
{
    [UIController("UI/Panel/pfMiddlePanel@embeded")]
    public class ProgressMapController : MonoBehaviour
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<MainMenuPanel>();
        [Inject]
        private LevelService _levelService;
        
        [Inject] 
        private UIService _uiService;
        
        private List<LevelViewModel> levelViewModels;

        private List<ProgressMapItemController> progressMapItemController = new List<ProgressMapItemController>();
        
        [UICreated]
        public void Init()
        {
            _levelService.AddListener<LevelEvent>(LevelEvent.UPDATED, OnLevelMapUpdated);
            CreateSpots();
        }
        
        private void OnLevelMapUpdated(LevelEvent levelEvent)
        {
            UpdateSpots();
        }

        private void CreateSpots()
        {
            levelViewModels = _levelService.GetLevels();
            foreach (LevelViewModel item in levelViewModels)
            {
                GameObject levelContainer = GameObject.Find($"level{item.LevelDescriptor.Order}");
                _uiService.Create<ProgressMapItemController>(UiModel
                            .Create<ProgressMapItemController>(item, item.LevelDescriptor.Order == _levelService.GetNextLevel(), item.LevelDescriptor.Order% 5==0)
                            .Container(levelContainer))
                        .Then(controller => progressMapItemController.Add(controller))
                        .Done();
            }
        }

        private void UpdateSpots()
        {
            _logger.Debug("update");
            levelViewModels = _levelService.GetLevels();
            foreach (ProgressMapItemController spotController in progressMapItemController)
            {
                LevelDescriptor descriptor = spotController.LevelViewModel.LevelDescriptor;
                
                LevelViewModel model = levelViewModels.Find(x => x.LevelDescriptor.Id.Equals(descriptor.Id));
                spotController.UpdateSpot(model, descriptor.Order == _levelService.GetNextLevel());
            }
        }
    }
}