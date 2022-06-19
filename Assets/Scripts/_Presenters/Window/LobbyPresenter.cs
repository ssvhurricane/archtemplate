using Services.Anchor;
using Services.Factory;
using Services.Log;
using Services.Window;
using System.Linq;
using UnityEngine;
using View.Window;
using Zenject;

namespace Presenters.Window
{
    public class LobbyPresenter
    {
        private readonly SignalBus _signalBus;
        private readonly LogService _logService;
        private readonly IWindowService _windowService;
        
        private readonly FactoryService _factoryService;
        private readonly HolderService _holderService;

        private LobbyView _lobbyView;

        public LobbyPresenter(SignalBus signalBus,
            LogService logService,
            IWindowService windowService,
            FactoryService factoryService,
            HolderService holderService) 
        {
            _signalBus = signalBus;
            _logService = logService;
            _windowService = windowService;

            _factoryService = factoryService;
            _holderService = holderService;

            _logService.ShowLog(GetType().Name,
                Services.Log.LogType.Message, 
                "Call Constructor Method.", 
                LogOutputLocationType.Console);
        }

        public void ShowView()
        {
            if (_windowService.IsWindowShowing<LobbyView>()) return;

            if (_windowService.GetWindow<LobbyView>() != null)
                _lobbyView = (LobbyView)_windowService.ShowWindow<LobbyView>();
            else
            {
                Transform holderTansform = _holderService._windowTypeHolders.FirstOrDefault(holder => holder.Key == WindowType.BaseWindow).Value;

                if (holderTansform != null)
                    _lobbyView = _factoryService.Spawn<LobbyView>(holderTansform);
            }
        }

        public IWindow GetView() 
        {
            return _lobbyView;
        }
    }
}
