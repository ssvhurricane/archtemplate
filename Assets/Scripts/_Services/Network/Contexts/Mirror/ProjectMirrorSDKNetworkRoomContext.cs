using Constants;
using Cysharp.Threading.Tasks;
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
        public void Construct(DiContainer temp, ISceneService sceneService)
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
            // Setup
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
        public override async void ServerChangeScene(string newSceneName)
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
