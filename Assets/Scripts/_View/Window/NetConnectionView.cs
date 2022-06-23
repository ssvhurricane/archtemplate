using System;
using Services.Window;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

namespace View.Window
{
    [Window("Resources/Prefabs/Windows/NetConnectionView", WindowType.PopUpWindow)]
    public class NetConnectionView : PopUpWindow
    {
        [SerializeField] protected WindowType Type;

        [SerializeField] public Button BackButton; // TODO: ref;

        [SerializeField] protected Button ConnectionButton;

        [SerializeField] protected TMP_Dropdown DropdownSelectAs;

        [SerializeField] protected TMP_InputField InputFieldHostName;

        private SignalBus _signalBus;

        private IDisposable _disposableConnectButton;

        [Inject]
        public void Constrcut(SignalBus signalBus)
        {
            _signalBus = signalBus;

            WindowType = Type;

            _signalBus.Fire(new WindowServiceSignals.Register(this));

            OnDispose(_disposableConnectButton);

            _disposableConnectButton = ConnectionButton
           .OnClickAsObservable()
           .Subscribe(_ => signalBus.Fire(new NetworkServiceSignals.Connect(InputFieldHostName.text, Services.Network.NetworkConnectAsType.Server))); // TODO:

        }

        private void OnDispose(IDisposable disposable)
        {
            disposable?.Dispose();
        }
    }
}