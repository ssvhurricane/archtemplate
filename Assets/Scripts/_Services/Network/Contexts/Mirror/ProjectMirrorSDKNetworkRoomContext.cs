using Constants;
using Mirror;
using Services.Essence;
using UnityEngine;
using View;

namespace Services.Network
{
    public class ProjectMirrorSDKNetworkRoomContext : NetworkRoomManager, INetworkContext, IEssence
    {
        // TODO:
        public EssenceType EssenceType { get; set; }

        public bool IsShown { get; protected set; }

        public string Id { get; set; }
        public CreationMethod CreationMethod { get; set; }
        public OwnerType OwnerType { get; set; }

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
            autoStartServerBuild = false;

            offlineScene = SceneServiceConstants.MainMenu;
            onlineScene = SceneServiceConstants.Room;

            transform.SetParent(parent, false);
            transform.SetAsLastSibling();
        }

        public new void StartServer()
        {
            base.StartServer();
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
