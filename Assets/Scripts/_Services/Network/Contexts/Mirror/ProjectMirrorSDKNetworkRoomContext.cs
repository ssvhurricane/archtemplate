using Constants;
using Mirror;
using Services.Essence;
using Services.Scene;
using UnityEngine;
using View;
using Zenject;

namespace Services.Network
{
    public class ProjectMirrorSDKNetworkRoomContext : NetworkRoomManager, INetworkContext, IEssence
    {
        public EssenceType EssenceType { get; set; }

        public bool IsShown { get; protected set; }

        public string Id { get; set; }
        public CreationMethod CreationMethod { get; set; }
        public OwnerType OwnerType { get; set; }

        
        private ISceneService _sceneService;

        [Inject]
        public void Construct(ISceneService sceneService)
        {
            _sceneService = sceneService;
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

            //Debug.Log($"ClientChangeScene newSceneName: {newSceneName} networkSceneName{networkSceneName}");

            // Let client prepare for scene change
            OnClientChangeScene(newSceneName, sceneOperation, customHandling);

            // After calling OnClientChangeScene, exit if server since server is already doing
            // the actual scene change, and we don't need to do it for the host client
            if (NetworkServer.active)
                return;

            // set client flag to stop processing messages while loading scenes.
            // otherwise we would process messages and then lose all the state
            // as soon as the load is finishing, causing all kinds of bugs
            // because of missing state.
            // (client may be null after StopClient etc.)
            // Debug.Log("ClientChangeScene: pausing handlers while scene is loading to avoid data loss after scene was loaded.");
            NetworkClient.isLoadingScene = true;

            // Cache sceneOperation so we know what was requested by the
            // Scene message in OnClientChangeScene and OnClientSceneChanged
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
