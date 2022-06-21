using Bootstrap;
using Constants;
using Model;
using Presenters.Window;
using Services.Input;
using Services.Log;
using Services.Network;
using Services.Project;
using Signals;
using UnityEngine;
using View.Camera;
using Zenject;

namespace Presenters
{
    public class ProjectPresenter : IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly LogService _logService;

        private readonly MainMenuPresenter _mainMenuPresenter;
        private readonly ProjectModel _projectModel;
        private readonly ProjectService _projectService;
        private readonly NetworkService _networkService;

        private MainHUDPresenter _mainHUDPresenter;
        private PlayerPresenter _playerPresenter;
        private WolfPresenter _wolfPresenter;
        private CameraPresenter _cameraPresenter;
        private RoomPresenter _roomPresenter;

        private  InputService _inputService;

        public ProjectPresenter(SignalBus signalBus,
            LogService logService,
            MainMenuPresenter mainMenuPresenter,
            ProjectModel projectModel,
            ProjectService projectService,
            NetworkService networkService
           )
        {
            _signalBus = signalBus;
            _logService = logService;
            _mainMenuPresenter = mainMenuPresenter;
            _projectModel = projectModel;
            _projectService = projectService;
            _networkService = networkService;

            _logService.ShowLog(GetType().Name, 
                Services.Log.LogType.Message,
                "Call Constructor Method.", 
                LogOutputLocationType.Console);
           
            _signalBus.Subscribe<SceneServiceSignals.SceneLoadingCompleted>(data =>
            {
                if (data.Data == SceneServiceConstants.MainMenu)
                {
                    _logService.ShowLog(GetType().Name,
                          Services.Log.LogType.Message,
                          $"Subscribe SceneServiceSignals.SceneLoadingCompleted, Data = {data.Data}",
                          LogOutputLocationType.Console);

                    _inputService.ClearServiceValues();

                    _mainMenuPresenter.ShowView(_projectService.GetProjectType());
                }

                // Offline Levels.
                if (data.Data == SceneServiceConstants.OfflineLevel1)
                {
                    //_logService.ShowLog(GetType().Name,
                    //    Services.Log.LogType.Message,
                    //    $"Subscribe SceneServiceSignals.SceneLoadingCompleted, Data ={data.Data}",
                    //    LogOutputLocationType.Console);

                    CreateGame();
                    
                    Cursor.visible = false;
                }

                //if (data.Data == LevelConstants.OfflineLevel1)
                //{
                //  etc..
                //}

                // Onlime Levels.
                if (data.Data == SceneServiceConstants.Room)
                {
                    // TODO:
                    CreateRoom();
                }

                if (data.Data == SceneServiceConstants.OnlineLevel1)
                {
                    // TODO:
                    CreateGame();
                }

            });


            _signalBus.Subscribe<SceneServiceSignals.SceneLoadingStarted>(data =>
            {
                _logService.ShowLog(GetType().Name,
                             Services.Log.LogType.Message,
                             $"Subscribe SceneServiceSignals.SceneLoadingStarted, Data ={data.Data}",
                             LogOutputLocationType.Console);
            });
        }

        public void Initialize()
        {
            // Entry point. 

            _projectService.Configurate();

            _mainMenuPresenter.ShowView(_projectService.GetProjectType(),
                _networkService.GetNetworkAuthMode());
        }

        private void CreateRoom() 
        {
            // TODO:
            _logService.ShowLog(GetType().Name,
                           Services.Log.LogType.Message,
                           $"CreateLobby!",
                           LogOutputLocationType.Console);

            var sceneContextDynamic = SceneContext.Create();
            sceneContextDynamic.AddNormalInstaller(new GameInstaller());
            sceneContextDynamic.Awake();

            _roomPresenter = sceneContextDynamic.Container.Resolve<RoomPresenter>();
            _roomPresenter.ShowView();

            _playerPresenter = sceneContextDynamic.Container.Resolve<PlayerPresenter>();
            _playerPresenter.ShowView();
            _playerPresenter.GetView().GetGameObject().GetComponent<Rigidbody>().useGravity = false;

            //_wolfPresenter = sceneContextDynamic.Container.Resolve<WolfPresenter>();
            //_wolfPresenter.ShowView();

            _cameraPresenter = sceneContextDynamic.Container.Resolve<CameraPresenter>();
            _cameraPresenter.ShowView<TPSCameraView>(CameraServiceConstants.TPSCamera, _playerPresenter.GetView());


            //_inputService = sceneContextDynamic.Container.Resolve<InputService>();
            //_inputService.TakePossessionOfObject(_playerPresenter);
        }

        private void CreateGame()
        {
            var sceneContextDynamic = SceneContext.Create();
            sceneContextDynamic.AddNormalInstaller(new GameInstaller());
            sceneContextDynamic.Awake();

            _mainHUDPresenter = sceneContextDynamic.Container.Resolve<MainHUDPresenter>();
            _mainHUDPresenter.ShowView();

            _playerPresenter = sceneContextDynamic.Container.Resolve<PlayerPresenter>();
            _playerPresenter.ShowView();

            _wolfPresenter = sceneContextDynamic.Container.Resolve<WolfPresenter>();
            _wolfPresenter.ShowView();

            _cameraPresenter = sceneContextDynamic.Container.Resolve<CameraPresenter>();
            _cameraPresenter.ShowView<TopDownCameraView>(CameraServiceConstants.TopDownCamera, _playerPresenter.GetView());


            _inputService = sceneContextDynamic.Container.Resolve<InputService>();
            _inputService.TakePossessionOfObject(_playerPresenter);

            //5. Get Game Flow

            //6. Start Game
            StartGame();
        }

        private void StartGame()
        {
            _logService.ShowLog(GetType().Name,
                            Services.Log.LogType.Message,
                            "Call StartGame Method.",
                            LogOutputLocationType.Console);
        }

        private void PauseGame() { }
    }
}