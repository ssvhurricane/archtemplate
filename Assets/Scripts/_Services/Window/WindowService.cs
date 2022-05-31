using Services.Log;
using Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Services.Window
{
    public class WindowService : IWindowService
    {
        public List<IWindow> _registeredWindows { get; private set; }

        public Dictionary<Type, MemoryPool<IWindow>> _windowPools { get; private set; }// TODO:
      
        private readonly SignalBus _signalBus;
        private readonly LogService _logService;

        private IWindow _activeWindow;

        public WindowService(SignalBus signalBus, LogService logService) 
        {
            _signalBus = signalBus;
            _logService = logService;

            _registeredWindows = new List<IWindow>();

            _windowPools = new Dictionary<Type, MemoryPool<IWindow>>();

            _signalBus.Subscribe<WindowServiceSignals.Register>(signal => OnRegisterWindow(signal.Window));

            _logService.ShowLog(GetType().Name, Log.LogType.Message, "Call Constructor Method.", LogOutputLocationType.Console);
        }

        private void OnRegisterWindow(IWindow window) 
        {
            if(!_registeredWindows.Contains(window))
                _registeredWindows.Add(window);
        }

        public void AddItemWindowPools(KeyValuePair<Type, MemoryPool<IWindow>> pool)
        {
            if (!_windowPools.Contains(pool)) _windowPools.Add(pool.Key, pool.Value);
        }

        public void RemoveItemWindowPools(KeyValuePair<Type, MemoryPool<IWindow>> pool)
        {
            if (_windowPools.Contains(pool)) _windowPools.Remove(pool.Key);
        }

        public IWindow GetWindow<TWindow>() where TWindow : class, IWindow
        {
           IWindow window = null;

           if( _registeredWindows.Count != 0) 
                window = _registeredWindows.FirstOrDefault(windowItem => windowItem is TWindow);

            return window;
        }

        public IWindow GetActiveWindow<TWindow>() where TWindow : class, IWindow
        {
            return (_activeWindow is TWindow) ? _activeWindow : null;
        }
    
        public bool IsWindowShowing<TWindow>() where TWindow : class, IWindow
        {
            return _activeWindow != null && _activeWindow.GetType() == typeof(TWindow) ||
                  _registeredWindows.Count > 0 && _registeredWindows.Exists(window => window.GetType() == typeof(TWindow));
        }
        public bool IsWindowShowing(Type type) 
        {
            return _activeWindow != null && _activeWindow.GetType() == type ||
                  _registeredWindows.Count > 0 && _registeredWindows.Exists(window => window.GetType() == type);
        }

        public IWindow ShowWindow<TWindow>(IWindowArgs windowArgs = null) where TWindow : class, IWindow
        {
            var window = GetWindow<TWindow>();

            if (!CanActivateWindow(window)) return default;

            window.Arguments = windowArgs;

            window.Show();

            return window;
        }

        public IWindow ShowWindow(Type type, IWindowArgs windowArgs = null)
        {
            return default(IWindow); // TODO:
        }

        public void ShowWindowWithInput<TWindow, TInput>(TInput input, IWindowArgs windowArgs = null) where TInput : IWindowInput where TWindow : class, IWindowWithInput<TInput>
        {
            IWindow window = GetWindow<TWindow>();

            window.Arguments = windowArgs;

            window.Show();
            // TODO:
        }

        public void HideWindow<TWindow>() where TWindow : class, IWindow 
        {
            IWindow window = GetWindow<TWindow>();
            
            if (window != null)
            {
                window.Hide();

                _signalBus.Fire(new WindowServiceSignals.Hidden(window));
            }
        }

        public void HideWindow(Type type)
        {
           // TODO:
        }

        public void HideActiveWindow()
        {
            if (_activeWindow == null) return;

            _activeWindow.Hide();

            _signalBus.Fire(new WindowServiceSignals.Hidden(_activeWindow));
        } 
        
        public void HideAllWindows()
        {
            if (_registeredWindows.Count != 0)
                foreach (var regWindow in _registeredWindows)  regWindow.Hide();
        } 

        private bool CanActivateWindow(IWindow window) 
        {
            var result = window != null && (_activeWindow == null || _activeWindow != window || _activeWindow.IsShown);

            if (!result) return false;

            return result;
        }
     
        public void ClearServiceValues()
        {
            _registeredWindows?.Clear();

            _windowPools?.Clear();
        }
    }
}