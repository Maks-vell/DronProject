using System.Text;
using AgkUI.Binding.Attributes;
using AgkUI.Binding.Attributes.Method;
using AgkUI.Dialog.Service;
using AgkUI.Element.Buttons;
using AgkUI.Element.Text;
using DronDonDon.Core.Audio;
using DronDonDon.Core.Audio.Model;
using DronDonDon.Core.Audio.Service;
using DronDonDon.Game.LevelDialogs;
using DronDonDon.Location.Model;
using DronDonDon.World;
using DronDonDon.World.Event;
using IoC.Attribute;
using IoC.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace DronDonDon.Location.UI
{
    [UIController("UI/Dialog/pfDronState@embeded")]
    public class DronStatsDialog :MonoBehaviour
    {
        [Inject]
        private IoCProvider<DialogManager> _dialogManager;
        
        [Inject]
        private IoCProvider<GameWorld> _gameWorld;

        [Inject] 
        private SoundService _soundService;

        [UIComponentBinding("CountChips")] 
            private UILabel _countChips;

        [UIComponentBinding("ParceTime")] 
            private UILabel _timer;
            
        [UIComponentBinding("CountEnergy")] 
            private UILabel _countEnergy;
            
        [UIComponentBinding("CountDurability")] 
            private UILabel _durability;

        [UIComponentBinding("ShieldButton")]
            private UIButton _shieldButton;
        
        [UIComponentBinding("SpeedButton")]
            private UIButton _speedButton;
            
        [UIObjectBinding("ShieldActive")]
            private GameObject _shieldActive;
            
        

        private float _time=0;

        private bool _isGame=false;

        private float _MaxDurability=0;
        
        [UICreated]
        private void Init(DronStats dronStats)
        {
            _MaxDurability = dronStats._durability;    //для вывода в процентах
            SetStats(dronStats);                                                                                
            _timer.text = "0,00";
            _shieldButton.gameObject.SetActive(false);
            _speedButton.gameObject.SetActive(false);
            _shieldActive.SetActive(false);
            _gameWorld.Require().AddListener<WorldObjectEvent>(WorldObjectEvent.UI_UPDATE, UiUpdate);
            _gameWorld.Require().AddListener<WorldObjectEvent>(WorldObjectEvent.START_GAME, StartGame);
            _gameWorld.Require().AddListener<WorldObjectEvent>(WorldObjectEvent.END_GAME, EndGame);
            _gameWorld.Require().AddListener<WorldObjectEvent>(WorldObjectEvent.TAKE_BOOST, SetActiveBoost);
        }
        
        private void PlaySound(Sound sound)
        {
            _soundService.StopAllSounds();
            _soundService.PlaySound(sound);
            
        }

        private void SetActiveBoost(WorldObjectEvent objectEvent)
        {
            switch (objectEvent._typeBoost)
            {
                case WorldObjectType.SHIELD_BUSTER:
                    _shieldButton.gameObject.SetActive(true);
                    break;
                case WorldObjectType.SPEED_BUSTER:
                    _speedButton.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void EndGame(WorldObjectEvent objectEvent)
        {
            _isGame = false;
        }

        private void StartGame(WorldObjectEvent objectEvent)
        {
            _isGame = true;
        }

        private void Update()
        {
            if (_isGame)
            {
                _time += Time.deltaTime;
                _timer.text = _time.ToString("F2");
            }
        }

        private void UiUpdate(WorldObjectEvent objectEvent)
        {
         SetStats(objectEvent._dronStats);   
        }

        private void SetStats(DronStats dronStats)
        {
            _countChips.text = dronStats._countChips.ToString();
            _countEnergy.text = dronStats._energy.ToString("F0");
            _durability.text = ((dronStats._durability / _MaxDurability) * 100).ToString("F0") + "%";
            
        }
        
        [UIOnClick("PauseButton")]
        private void OnPauseButton()
        {
            _dialogManager.Require().ShowModal<LevelPauseDialog>();
        }
        
        [UIOnClick("ShieldButton")]
        private void OnShieldButton()
        {
            PlaySound(GameSounds.SHIELD_ACTIVATED);
            _gameWorld.Require().Dispatch(new WorldObjectEvent(WorldObjectEvent.ACTIVATE_BOOST, 
                WorldObjectType.SHIELD_BUSTER));
            _shieldButton.gameObject.SetActive(false);
            _shieldActive.SetActive(true);
            //_shieldActive.GetComponent<Blinking>()._on = true;
            Invoke(nameof(DisableShieldImage), 5 );
        }

        private void DisableShieldImage()
        {
            //_shieldActive.GetComponent<Blinking>()._on = false;
            _shieldActive.SetActive(false);
        }
        
        [UIOnClick("SpeedButton")]
        private void OnSpeedButton()
        {
            PlaySound(GameSounds.SPEED_ACTIVATED);
            _gameWorld.Require().Dispatch(new WorldObjectEvent(WorldObjectEvent.ACTIVATE_BOOST, 
                WorldObjectType.SPEED_BUSTER));
            _speedButton.gameObject.SetActive(false);
        }
    }
}