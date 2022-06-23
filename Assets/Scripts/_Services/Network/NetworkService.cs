using Data.Settings;
using Signals;
using Zenject;

namespace Services.Network
{
    public class NetworkService
    {
        private readonly SignalBus _signalBus;

        private MirrorSDKController _mirrorSDKController;

        private CustomSDKController _customSDKController;

        private NetworkServiceSettings _networkServiceSettings;

        public NetworkService(SignalBus signalBus,
            MirrorSDKController mirrorSDKController,
            CustomSDKController customSDKController,
            NetworkServiceSettings networkServiceSettings)
        {
            _signalBus = signalBus;

            _mirrorSDKController = mirrorSDKController;

            _customSDKController = customSDKController;

            _networkServiceSettings = networkServiceSettings;

            _signalBus.Subscribe<NetworkServiceSignals.Connect>(signal =>
            {
                OnConnect(signal.Host, signal.NetworkConnectAsType);
            });

            _signalBus.Subscribe<NetworkServiceSignals.Disconnect>(signal =>
            {
                OnDisconnect();
            });
        }

        public void Initialize()
        {
            switch ( _networkServiceSettings.NetworkEngine)
            {
                case NetworkEngine.Mirror:
                    {
                        // TODO:

                        _mirrorSDKController.CreateContext(_networkServiceSettings.NetworkContextType);

                        break;
                    }
                case NetworkEngine.Custom: 
                    {
                        // TODO:  CustomSDKController
                        break;
                    }
            }
        }

        public NetworkType GetNetworkType() 
        {
            return _networkServiceSettings.NetworkType;
        }

        public NetworkEngine GetNetworkEngine() 
        { 
            return _networkServiceSettings.NetworkEngine; 
        }

        public NetworkAuthMode GetNetworkAuthMode()
        { 
            return _networkServiceSettings.NetworkAuthMode;
        }

        private void OnConnect(string hostName, NetworkConnectAsType networkConnectAsType)
        {
            // TODO:
            StartServer();
        }

        private void OnDisconnect()
        {
            // TODO:
        }

        private void StartServer()
        {
            _mirrorSDKController.GetCurrnetNetworkContext().StartServer();
        }

        private void StartClient()
        {
            // TODO:
        }
    
    }
}
