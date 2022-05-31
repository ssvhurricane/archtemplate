using Zenject;

namespace Services.Network
{
    public class NetworkService
    {
        private readonly SignalBus _signalBus;

        public NetworkService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    }
}
