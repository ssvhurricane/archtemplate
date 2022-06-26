using Constants;
using Mirror;
using Services.Essence;
using Services.Scene;
using UnityEngine;
using View;
using Zenject;
using Mirror.Examples.NetworkRoom;
using Presenters;
using Services.Input;

namespace Services.Network
{
    public class ProjectMirrorSDKNetworkRoomContext : NetworkRoomManagerExtension, INetworkContext, IEssence
    {
        public EssenceType EssenceType { get; set; }

        public bool IsShown { get; protected set; }

        public string Id { get; set; }
        public CreationMethod CreationMethod { get; set; }
        public OwnerType OwnerType { get; set; }
        
        private ISceneService _sceneService;
        private PlayerPresenter _playerPresenter;
        private CameraPresenter _cameraPresenter;
        private InputService _inputService;

        [Inject]
        public void Construct(ISceneService sceneService,
            PlayerPresenter playerPresenter,
            CameraPresenter cameraPresenter,
            InputService inputService
            )
        {
            _sceneService = sceneService;

            _playerPresenter = playerPresenter;

            _cameraPresenter = cameraPresenter;

            _inputService = inputService;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
       
        public void Hide()
        {
            gameObject.SetActive(false);
            IsShown = false;
        }

        public void Initialize(Transform parent)
        {
            dontDestroyOnLoad = true;
            runInBackground = true;
            autoStartServerBuild = true;

            offlineScene = SceneServiceConstants.MainMenu;
            onlineScene = SceneServiceConstants.Room;

            RoomScene =  SceneServiceConstants.Room;
            GameplayScene = SceneServiceConstants.OnlineLevel1;

            transform.SetParent(parent, false);
            transform.SetAsLastSibling();
        }

        public new void StartServer()
        {
            base.StartServer();
        }

        /// <summary>
        /// Need ovverride for use SceneService.
        /// </summary>
        /// <param name="newSceneName"></param>
        /// <param name="sceneOperation"></param>
        /// <param name="customHandling"></param>
        public override void ClientChangeScene(string newSceneName, SceneOperation sceneOperation = SceneOperation.Normal, bool customHandling = false)
        {
            // TODO:
            if (string.IsNullOrWhiteSpace(newSceneName))
            {
                Debug.LogError("ClientChangeScene empty scene name");
                return;
            }
      
            OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            
            if (NetworkServer.active)
                return;
           
            NetworkClient.isLoadingScene = true;
            
            clientSceneOperation = sceneOperation;

            // scene handling will happen in overrides of OnClientChangeScene and/or OnClientSceneChanged
            // Do not call FinishLoadScene here. Custom handler will assign loadingSceneAsync and we need
            // to wait for that to finish. UpdateScene already checks for that to be not null and isDone.
            // if (customHandling)
            //     return;

            loadingSceneAsync = _sceneService.LoadLevelBase(newSceneName);

            // don't change the client's current networkSceneName when loading additive scene content
            if (sceneOperation == SceneOperation.Normal)
                networkSceneName = newSceneName;
        }

        /// <summary>
        /// Need ovverride for use SceneService.
        /// </summary>
        /// <param name="newSceneName"></param>
        public override void ServerChangeScene(string newSceneName)
        {
            if (string.IsNullOrWhiteSpace(newSceneName))
            {
                Debug.LogError("ServerChangeScene empty scene name");
                return;
            }

            if (NetworkServer.isLoadingScene && newSceneName == networkSceneName)
            {
                Debug.LogError($"Scene change is already in progress for {newSceneName}");
                return;
            }

            // Debug.Log($"ServerChangeScene {newSceneName}");
            NetworkServer.SetAllClientsNotReady();
            networkSceneName = newSceneName;

            // Let server prepare for scene change
            OnServerChangeScene(newSceneName);

            // set server flag to stop processing messages while changing scenes
            // it will be re-enabled in FinishLoadScene.
            NetworkServer.isLoadingScene = true;

            loadingSceneAsync = _sceneService.LoadLevelBase(newSceneName);
          
            // ServerChangeScene can be called w hen stopping the server
            // when this happens the server is not active so does not need to tell clients about the change
            if (NetworkServer.active)
            {
                // notify all clients about the new scene
                NetworkServer.SendToAll(new SceneMessage { sceneName = newSceneName });
            }

            startPositionIndex = 0;
            startPositions.Clear();
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;

               base.ServerChangeScene(GameplayScene);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="roomPlayer"></param>
        public override void SceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            // TODO: This temp, meed move to project presenter.
            // Debug.Log($"NetworkRoom SceneLoadedForPlayer scene: {SceneManager.GetActiveScene().path} {conn}");
            
            if (IsSceneActive(RoomScene))
            {
                // cant be ready in room, add to ready list
                PendingPlayer pending;
                pending.conn = conn;
                pending.roomPlayer = roomPlayer;
                pendingPlayers.Add(pending);
                return;
            }

            GameObject gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);

            if (gamePlayer == null)
            {
                Transform startPos = GetStartPosition();
             
                if (startPos != null) _playerPresenter.ShowView(playerPrefab, startPos);
                else _playerPresenter.ShowView();

                gamePlayer = _playerPresenter.GetView().GetGameObject();
            }
           
            if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
                return;
            
            // replace room player with game player
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
            
        }
        
        public new void StopServer()
        {
            base.StopServer();
        }

        public new void StartClient()
        {
            base.StartClient();
        }


        public new void StopClient()
        {
            base.StopClient();
        }

        public new void StartHost()
        {
            base.StartHost();
        }

        public new void StopHost()
        {
            base.StopHost();
        }

        public void Show()
        {
            IsShown = true;
            gameObject.SetActive(true);
        }
    }
}
