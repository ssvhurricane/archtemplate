namespace Services.Network
{
    public interface INetworkContext 
    {
        public void StartServer();

        public void StopServer();
    }
}