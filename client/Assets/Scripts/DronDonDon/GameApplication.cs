using Adept.Logger;
using AgkCommons.Event;
using AgkCommons.Util;
using AgkUI.Screens.Service;
using DronDonDon.Core.Event;
using DronDonDon.Core.Filter;
using IoC;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace DronDonDon
{
    public class GameApplication : GameEventDispatcher
    {
        private static readonly IAdeptLogger _logger = LoggerFactory.GetLogger<GameApplication>();

        private const string SCENE_NAME = "MainScene";
        
        [PublicAPI]
        public static GameApplication Instance { get; private set; }
        
        public bool FiltersInitialized { get; set; }
        
        public void Restart()
        {
            _logger.Info("Restart App!!!");
            AppContext.Resolve<ScreenStructureManager>().DestroyScreen();
            AppContext.Clear();
            DestroyImmediate(gameObject);

            SceneManager.LoadScene(SCENE_NAME, LoadSceneMode.Single);
        }

        private void Awake()
        {
            FiltersInitialized = false;
            Instance = this;

#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += HandleOnPlayModeChanged;
            void HandleOnPlayModeChanged(PauseState pauseState)
            {
                Dispatch(new GamePausedEvent(GamePausedEvent.GAME_PAUSED, pauseState == PauseState.Paused));
            }
#endif
            UnityMainThreadDispatcher.AddDispatcherToScene();
            DontDestroyOnLoad(gameObject);
            RunFilter();
        }

        private void RunFilter()
        {
            AppFilterChain filterChain = gameObject.AddComponent<AppFilterChain>();
            filterChain.AddFilter(new IoCFilter());
            filterChain.AddFilter(new InitScreenFilter());
            filterChain.AddFilter(new ConfigLoadFilter());
            filterChain.AddFilter(new ConsoleFilter());
            filterChain.AddFilter(new ConfigureServiceFilter());
            filterChain.AddFilter(new AppSettingsFilter());
            filterChain.AddFilter(new StartGameFilter());
            filterChain.AddFilter(new InitableFilter());
            filterChain.Next();
        }
    }
}