using Mirror;
using Services.Essence;
using UnityEngine;
using View;

namespace Services.Network
{
    public class ProjectMirrorSDKNetworkContext : NetworkManager, INetworkContext, IEssence
    {
        // TODO:
        public EssenceType EssenceType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public bool IsShown => throw new System.NotImplementedException();

        public string Id { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public CreationMethod CreationMethod { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public OwnerType OwnerType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public GameObject GetGameObject()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(Transform parent)
        {
            throw new System.NotImplementedException();
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }
    }
}