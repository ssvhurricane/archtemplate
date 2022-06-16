using Data.Settings;
using Zenject;

namespace Services.Network
{
    public class NetworkService
    {
        private readonly SignalBus _signalBus;

        private NetworkServiceSettings _networkServiceSettings;
        public NetworkService(SignalBus signalBus, NetworkServiceSettings networkServiceSettings)
        {
            _signalBus = signalBus;

            _networkServiceSettings = networkServiceSettings;
        }

        public void Initialize()
        {
            switch ( _networkServiceSettings.NetworkEngine)
            {
                case NetworkEngine.Mirror:
                    {
                        // TODO:
                       // MirrorSDKController
                        break;
                    }
                case NetworkEngine.Custom: 
                    {
                        // TODO: 
                        // CustomSDKController
                        break;
                    }
            }
        }
    }
}
