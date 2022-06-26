using Constants;
using Mirror;
using Presenters;
using Services.Essence;
using Services.Input;
using Signals;
using TMPro;
using UnityEngine;
using View.Camera;
using Zenject;

namespace View
{
    public class PlayerView : NetworkEssence
    {
        [SerializeField] public TextMeshProUGUI PlayerName;

        [SerializeField] public Animator Animator;
        [SerializeField] protected EssenceType Layer;

        [SerializeField] public GameObject FirstJointHand;
        [SerializeField] public GameObject FirstJointBack;

        [SerializeField] public GameObject SecondJointHand;
        [SerializeField] public GameObject SecondJointBack;

        private SignalBus _signalBus;
       
        private CameraPresenter _cameraPresenter;
        
        private PlayerPresenter _playerPresenter;
        
        private InputService _inputService;

        [Inject]
        public void Constrcut(SignalBus signalBus)
        {
            _signalBus = signalBus;

            EssenceType = Layer;

            signalBus.Fire(new EssenceServiceSignals.Register(this));
           
        }
       

        public void NetInitView(CameraPresenter cPresenter, PlayerPresenter _pPresenter, InputService inService)
        {
            _cameraPresenter = cPresenter;
            _playerPresenter = _pPresenter;
            _inputService = inService;
        }

        public override void OnStartLocalPlayer()
        {
            if (_signalBus == null) Debug.Log("Bus null");
            
            //_cameraPresenter.ShowView<TopDownCameraView>(CameraServiceConstants.TopDownCamera, _playerPresenter.GetView());
            
           // _wolfPresenter.ShowView();

          //  _inputService.TakePossessionOfObject(_playerPresenter);
           
        }

        public override void OnStopLocalPlayer()
        {
           
        }
    }
}
