using System;
using System.Collections.Generic;
using System.Reflection;
using DronDonDon.Location.Model.Dron;
using UnityEngine;
using DronDonDon.Location.Model;
using AgkCommons.Event;
using AgkCommons.Input.Gesture.Model;
using AgkCommons.Input.Gesture.Model.Gestures;
using IoC.Attribute;
using AgkCommons.Input.Gesture.Service;
using BezierSolution;
using DronDonDon.Core.Audio;
using DronDonDon.Core.Audio.Model;
using DronDonDon.Core.Audio.Service;
using DronDonDon.Location.Service;
using DronDonDon.Location.World.Dron.Service;
using DronDonDon.World;
using DronDonDon.World.Event;
using IoC.Util;

namespace DronDonDon.Location.World.Dron
{
    public class DronController: GameEventDispatcher,  IWorldObjectController<DronModel>
    {
        private Vector2 _containerPosition=Vector2.zero;
        private BezierWalkerWithSpeed _bezier;
        private float _containerCoefficient=1;
        private bool _isShifting=false;
        private Vector3 _previusPosition;
        private float _shiftCoeficient=0;
        private float _levelSpeed = 10;
        private float _acceleration = 0.2f;
        private bool _isGameRun = false;
        private float _angleRotate = 60;
        private Swipe _lastWorkSwipe=null;
        private DronModel _model=null;
        private float _boostSpeed=0;
        
        private float _speedShift=0;

        [Inject]
        private IGestureService _gestureService;
        
        [Inject]
        private IoCProvider<GameWorld> _gameWorld;
        
        [Inject]
        private GameService _gameService;
        
        [Inject] 
        private SoundService _soundService;
        
        public WorldObjectType ObjectType { get; private set; }
        public void Init(DronModel  model)
        {
            _bezier = transform.parent.transform.GetComponentInParent<BezierWalkerWithSpeed>();
            DisablePath();
            ObjectType = model.ObjectType;
            _model = model;
            _gameWorld.Require().AddListener<WorldObjectEvent>(WorldObjectEvent.START_GAME, StartGame);
            _gameWorld.Require().AddListener<WorldObjectEvent>(WorldObjectEvent.DRON_BOOST_SPEED, Acceleration);
            _gestureService.AddSwipeHandler(OnSwiped,false);
        }
        
        private void StartGame(WorldObjectEvent worldObjectEvent)
        {
            _isGameRun = true;
            EnablePath();
            _speedShift = _gameService.SpeedShift;
        }
        
        public void Update()
        {
            if (!_isGameRun) return;
            
            if (GetBeizerPassedPath() < 0.5f)
            {
                _levelSpeed += _acceleration *Time.deltaTime;
            }
            SetBeizerSpeed(_levelSpeed);
            
            if (_isShifting)
            {
                _shiftCoeficient += _speedShift * Time.deltaTime;
                transform.localPosition = Vector3.Lerp(_previusPosition, _containerPosition, _shiftCoeficient);
                RotateSelf(_lastWorkSwipe, (float)Math.Sin(_shiftCoeficient * Math.PI) * _angleRotate);
                
                if (transform.localPosition.Equals(_containerPosition))
                {
                    _isShifting = false;
                    _shiftCoeficient = 0;
                    RotateSelf(_lastWorkSwipe, 0);
                }
            }
        }
        
        private void SetBeizerSpeed(float newSpeed)
        {
            _bezier.speed = newSpeed;
        }
        
        private float GetBeizerPassedPath()
        {
            return _bezier.NormalizedT;
        }

        private void EnablePath()
        {
            _bezier.enabled=true;
        }
        private void DisablePath()
        {
            _bezier.enabled=false;
        }
        
        private void PlaySound(Sound sound)
        {
            _soundService.StopAllSounds();
            _soundService.PlaySound(sound);
        }

        
        private void OnSwiped(Swipe swipe)
        {
            if (!_isShifting && _isGameRun)
            {
                _lastWorkSwipe = swipe;
                ShiftNewPosition(NumberSwipedToSector(swipe));
                PlaySound(GameSounds.SHIFT);
            }
        }
        
        private int NumberSwipedToSector(Swipe swipe)
        {
            Vector2 swipeEndPoint;
            Vector2 swipeVector;
            int angle;
            int result;
         
            swipeEndPoint = (Vector2) typeof(Swipe).GetField("_endPoint", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(swipe);
            swipeVector = swipeEndPoint - swipe.Position;
            angle = (int) Vector2.Angle(Vector2.up, swipeVector.normalized);
            
            result = Vector2.Angle(Vector2.right, swipeVector.normalized) > 90 ? 360 - angle : angle;
            return (int) Mathf.Round(result / 45f) % 8;
        }
    
       
        private Dictionary<int, Vector2> virtualVectors = new Dictionary<int, Vector2>(8)
        {
            {0, new Vector2(0, 1)},    // вверх
            {1, new Vector2(1, 1)},    // вправо вверх
            {2, new Vector2(1, 0)},    // вправо
            {3, new Vector2(1, -1)},   // вправо вниз
            {4, new Vector2(0, -1)},   // вниз
            {5, new Vector2(-1, -1)},  // влево вниз
            {6, new Vector2(-1, 0)},   // влево 
            {7, new Vector2(-1, 1)},   // влево вверх
        };
        
        private bool ValidateMovement(int sector)
        {
            Vector2Int fakePosition = Vector2Int.RoundToInt(_containerPosition / _containerCoefficient); 
            return sector switch
            {
                0 => fakePosition.y != 1,
                1 => fakePosition.x != 1 && fakePosition.y != 1,
                2 => fakePosition.x != 1,
                3 => fakePosition.x != 1 && fakePosition.y != -1,
                4 => fakePosition.y != -1,
                5 => fakePosition.x != -1 && fakePosition.y != -1,
                6 => fakePosition.x != -1,
                7 => fakePosition.x != -1 && fakePosition.y != 1,
                _ => false
            };
        }

        private void ShiftNewPosition(int sector)
        {
            if (!ValidateMovement(sector))
            {
                return;
            }
            _containerPosition += virtualVectors[sector] *_containerCoefficient;
            _previusPosition = transform.localPosition;
            _isShifting = true;
        }

        private void RotateSelf(Swipe swipe, float angleRotate)
        {
            if (swipe.Check(HorizontalSwipeDirection.LEFT))
            {
                transform.localRotation = Quaternion.Euler(0,0, angleRotate);
            }
            else if (swipe.Check(HorizontalSwipeDirection.RIGHT))
            {
                transform.localRotation = Quaternion.Euler(0,0, -angleRotate);
            }
            else if (swipe.Check(VerticalSwipeDirection.DOWN))
            {
                transform.localRotation = Quaternion.Euler(angleRotate * 0.5f, 0,0);
            }
            else if (swipe.Check(VerticalSwipeDirection.UP))
            {
                transform.localRotation = Quaternion.Euler( -angleRotate * 0.5f, 0,0);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            _gameWorld.Require().Dispatch(new WorldObjectEvent(WorldObjectEvent.ON_COLLISION, 
                other.gameObject));
        }

        public void Acceleration( WorldObjectEvent objectEvent)
        {
            _boostSpeed = objectEvent.SpeedBoost;
            _levelSpeed += _boostSpeed;
            Invoke(nameof(DisableAcceleration),objectEvent.SpeedBoostTime);
        }

        public void DisableAcceleration()
        {
            _levelSpeed -= _boostSpeed;
        }
    }
}