using System.Linq;
using Services.Anchor;
using Services.Essence;
using Services.Factory;
using Services.Log;
using Services.Scene;
using UnityEngine;
using Zenject;

namespace Services.Network
{
    public class MirrorSDKController
    {
        private readonly SignalBus _signalBus;
        private readonly LogService _logService;
        private readonly ISceneService _sceneService;
        private readonly EssenceService _essenceService;
        private readonly FactoryService _factoryService;
        private readonly HolderService _holderService;

        private ProjectMirrorSDKNetworkContext _projectMirrorSDKNetworkContext = null;
        private ProjectMirrorSDKNetworkRoomContext _projectMirrorSDKNetworkRoomContext = null;

        public MirrorSDKController(SignalBus signalBus,
            LogService logService,
            ISceneService sceneService,
            EssenceService essenceService,
            FactoryService factoryService,
            HolderService holderService)
        {
            _signalBus = signalBus;

            _logService = logService;

            _sceneService = sceneService;

            _essenceService = essenceService;

            _factoryService = factoryService;

            _holderService = holderService;
        }

        public void CreateContext(NetworkContextType networkContextType)
        {
            switch (networkContextType)
            {
                case NetworkContextType.Basic:
                    {
                        BasicContextType(ref _projectMirrorSDKNetworkContext);

                        break;
                    }
                case NetworkContextType.Room:
                    {
                        RoomContextType(ref _projectMirrorSDKNetworkRoomContext);

                        break;
                    }
            }
        }

        private void BasicContextType(ref ProjectMirrorSDKNetworkContext projectMirrorSDKNetworkContext)
        {
            // TODO:
        }

        private void RoomContextType(ref ProjectMirrorSDKNetworkRoomContext projectMirrorSDKNetworkRoomContext)
        {
            if (_essenceService.IsEssenceShowing<ProjectMirrorSDKNetworkRoomContext>()) return;

            if (_essenceService.GetEssence<ProjectMirrorSDKNetworkRoomContext>() != null)
                projectMirrorSDKNetworkRoomContext = (ProjectMirrorSDKNetworkRoomContext)_essenceService.ShowEssence<ProjectMirrorSDKNetworkRoomContext>();
            else
            {
                Transform holderTansform = _holderService._essenceTypeTypeHolders.FirstOrDefault(holder => holder.Key == EssenceType.ContextGameObject).Value;

                if (holderTansform != null)
                    projectMirrorSDKNetworkRoomContext = _factoryService.Spawn<ProjectMirrorSDKNetworkRoomContext>(holderTansform);

            }
        }
    }
}