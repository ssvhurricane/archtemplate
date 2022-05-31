using System;
using Config;
using Services.Network;

namespace Data.Settings
{
    [Serializable]
    public class NetworkServiceSettings : IRegistryData
    {
        public string Id;

        public NetworkType NetworkType;

        string IRegistryData.Id => Id;
    }
}
