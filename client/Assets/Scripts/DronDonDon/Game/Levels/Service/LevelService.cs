using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AgkCommons.Configurations;
using AgkCommons.Event;
using AgkCommons.Resources;
using AgkUI.Dialog.Service;
using DigitalRubyShared;
using DronDonDon.Billing.Service;
using DronDonDon.Core.Filter;
using DronDonDon.Game.Levels.Descriptor;
using DronDonDon.Game.Levels.Event;
using DronDonDon.Game.Levels.IoC;
using DronDonDon.Game.Levels.Model;
using DronDonDon.Game.Levels.Repository;
using DronDonDon.Resource.UI.DescriptionLevelDialog;
using IoC.Attribute;
using IoC.Util;

namespace DronDonDon.Game.Levels.Service
{
    public class LevelService : GameEventDispatcher, IInitable  
    {
        [Inject]
        private ResourceService _resourceService;
        
        [Inject] 
        private ProgressRepository _progressRepository;

        [Inject]
        private LevelDescriptorRegistry _levelDescriptorRegistry;
        
        [Inject]
        private IoCProvider<DialogManager> _dialogManager;

        [Inject] 
        private BillingService _billingService;
        
        private List<LevelViewModel> _levelsViewModels  = new List<LevelViewModel>();
        public string CurrentLevelId { get; set; }

        public void Init()
        {
            if (_levelDescriptorRegistry.LevelDescriptors.Count == 0)
            {
                _resourceService.LoadConfiguration("Configs/levels@embeded", OnConfigLoaded);
            }
        }
        
        public void ShowStartLevelDialog(string leveId)
        {
            LevelDescriptor levelDescriptor = _levelsViewModels.Find(x => x.LevelDescriptor.Id.Equals(leveId)).LevelDescriptor;
            _dialogManager.Require().Show<DescriptionLevelDialog>(levelDescriptor);
        }
        

        public int GetNextLevel()
        {
            List<LevelDescriptor> descriptors = _levelDescriptorRegistry.LevelDescriptors.OrderBy(o=>o.Order).ToList();
            foreach (LevelDescriptor descriptor in descriptors)
            {
                LevelProgress progress = GetPlayerProgressModel().LevelsProgress.FirstOrDefault(a => a.Id == descriptor.Id);
                if (progress == null)
                {
                    return descriptor.Order;
                }
            }

            return 0;
        }



        public void SetLevelProgress(string levelId, int countStars, int countChips, float transitTime, int durability)
        {
            PlayerProgressModel model = GetPlayerProgressModel();
            LevelProgress levelProgress = model.LevelsProgress.FirstOrDefault(a => a.Id == levelId);
            if (levelProgress == null)
            {
                levelProgress = new LevelProgress(){Id = levelId};
                model.LevelsProgress.Add(levelProgress);
            }
            //model.LevelsProgress.Find(x => x.id == )
            levelProgress.CountChips = countChips;
            if (levelProgress.CountStars < countStars)
            {
                levelProgress.CountStars = countStars;
            }
            levelProgress.TransitTime = transitTime;
            levelProgress.Durability = durability;
            _billingService.AddCredits(countChips);
            SaveProgress(model);
            //Dispatch(new LevelEvent(LevelEvent.UPDATED));
        }

        public LevelProgress GetLevelProgress(string levelId)
        {
            return GetLevelProgressById(levelId);
        }

        public int GetChipsCount(string levelId)
        {
            return GetLevelProgressById(levelId).CountChips;
        }
        
        public int GetStarsCount(string levelId)
        {
            return GetLevelProgressById(levelId).CountStars;
        }
        
        public float GetTransitTime(string levelId)
        {
            return GetLevelProgressById(levelId).TransitTime;
        }

        public List<LevelViewModel> GetLevels()
        {
            _levelsViewModels = new List<LevelViewModel>();
            PlayerProgressModel playerProgressModel = GetPlayerProgressModel();
            foreach (LevelDescriptor item in _levelDescriptorRegistry.LevelDescriptors)
            {
                LevelViewModel levelViewModel = new LevelViewModel();
                levelViewModel.LevelDescriptor = item;
                levelViewModel.LevelProgress = playerProgressModel.LevelsProgress.Find(x => x.Id.Equals(item.Id));
                _levelsViewModels.Add(levelViewModel);
            }
            return _levelsViewModels;
        }

        public void ResetPlayerProgress()
        {
            _progressRepository.Delete();
            Dispatch(new LevelEvent(LevelEvent.UPDATED));
        }

        public LevelProgress GetLevelProgressById(string levelId)
        {
            PlayerProgressModel playerModel = GetPlayerProgressModel();
            LevelProgress level = playerModel.LevelsProgress.Find(x => x.Id.Equals(levelId));
            return level;
        }
        
        public PlayerProgressModel GetPlayerProgressModel()
        {
            return !HasPlayerProgress() ? new PlayerProgressModel() : _progressRepository.Require();;
        }
        
        private void OnConfigLoaded(Configuration config, object[] loadparameters)
        {
            foreach (Configuration temp in config.GetList<Configuration>("levels.level"))
            {
                LevelDescriptor levelDescriptor = new LevelDescriptor();
                levelDescriptor.Configure(temp);
                _levelDescriptorRegistry.LevelDescriptors.Add(levelDescriptor);
            }
        }

        private void SaveProgress( PlayerProgressModel model)
        {
            _progressRepository.Set(model);
        }
        
        private bool HasPlayerProgress()
        {
            return _progressRepository.Exists();
        }
    }
}