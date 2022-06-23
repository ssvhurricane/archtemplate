using Mirror;
using Services.Essence;
using UnityEngine;
using View;

namespace Services.Network
{
    public class ProjectMirrorSDKNetworkContext : NetworkManager, INetworkContext, IEssence
    {
        // TODO:
        public EssenceType EssenceType { get; set; }

        public bool IsShown { get; protected set; } = false;

        public string Id { get; set; }
        public CreationMethod CreationMethod { get; set; }
        public OwnerType OwnerType { get; set; }

        public GameObject GetGameObject()
        {
            throw new System.NotImplementedException();
        }

        public void Show()
        {
           // TODO:
        }

        public void Hide()
        {
            // TODO:
        }

        public void Initialize(Transform parent)
        {
            // TODO:
        }

        public new void StartServer()
        {
            // TODO:
        }

        public new void StopServer()
        {
            // TODO:
        }

        public new void StartClient() { }

        public new void StopClient() { }

        public new void StartHost() { }

        public new void StopHost() { }

    }
}