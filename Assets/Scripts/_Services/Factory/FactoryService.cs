using Services.Essence;
using Services.Resources;
using Services.Window;
using Signals;
using System;
using System.Collections.Generic;
using UnityEngine;
using View;
using Zenject;

namespace Services.Factory
{
    public class FactoryService 
    {
        private readonly DiContainer _container;

        private readonly ResourcesService _resources;

        private readonly SignalBus _signalBus;

        private const string IEssence = "IEssence";

        private const string IWindow = "IWindow";
        public FactoryService(DiContainer container, ResourcesService resources, SignalBus signalBus) 
        {
            _container = container;

            _resources = resources;

            _signalBus = signalBus;
        }

        public TView Spawn<TView>(Transform parentTransform) where TView : class, IView
        {
            GameObject prefab = null;

            TView resultView = null;

            if (typeof(TView).GetInterface(IEssence) != null) 
            {
                prefab = _resources.GetResource(TypeResource.View, typeof(TView));

                if (prefab == null)
                {
                    Debug.LogWarning("[FactoryService] -> can't find essence for type : " + typeof(TView));
                    return null;
                }

                resultView = _container.InstantiatePrefabForComponent<TView>(prefab.gameObject); 
                
                if (resultView == null)
                {
                    Debug.LogError("[FactoryService] -> There is no view with type " + typeof(TView).Name);
                    return null;
                }

                ((IEssence)resultView).Initialize(parentTransform);
            }
            else if (typeof(TView).GetInterface(IWindow) != null) 
            {
                prefab = _resources.GetResource(TypeResource.Window, typeof(TView)); 
                
                if (prefab == null)
                {
                    Debug.LogWarning("[FactoryService] -> can't find window for type : " + typeof(TView));
                    return null;
                }

                resultView = _container.InstantiatePrefabForComponent<TView>(prefab.gameObject); 
                
                if (resultView == null)
                {
                    Debug.LogError("[FactoryService] -> There is no view with type " + typeof(TView).Name);
                    return null;
                }

                ((IWindow)resultView).Initialize(parentTransform);
            }

            return resultView;
        }
    }
}