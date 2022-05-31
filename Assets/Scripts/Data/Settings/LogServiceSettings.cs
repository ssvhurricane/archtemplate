using Config;
using System;
using UnityEngine;

namespace Data.Settings
{
    [Serializable]
    public class LogServiceSettings : IRegistryData
    {
        public string Id;

        public bool EnableAll;

        public LogItemData[] LogItemDatas;
        string IRegistryData.Id => Id;
    }

    [Serializable]
    public class LogItemData 
    {
        public string Name;

        public bool Enable;

        public Color Color;

        public LogItemType LogItemType;
    }

    public enum LogItemType
    {
        None,
        Presenter,
        Service,
        Model,
        View,
        Gameplay,
        Ability,
        Test
    }
}
