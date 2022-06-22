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
            transform.SetParent(parent, false);
            transform.SetAsLastSibling();
        }

        public new void StartServer()
        {
            base.StartServer();
            // TODO:
        }

        public new void StopServer()
        {
            // TODO:
        }

        public void Show()
        {
            IsShown = true;
            gameObject.SetActive(true);
        }
    }
}
