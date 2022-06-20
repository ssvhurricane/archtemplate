using Services.Essence;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace View
{
    public class PlayerView : BaseEssence
    {
        [SerializeField] public TextMeshProUGUI PlayerName;

        [SerializeField] public Animator Animator;
        [SerializeField] protected EssenceType Layer;

        [SerializeField] public GameObject FirstJointHand;
        [SerializeField] public GameObject FirstJointBack;

        [SerializeField] public GameObject SecondJointHand;
        [SerializeField] public GameObject SecondJointBack;

        private SignalBus _signalBus;

        [Inject]
        public void Constrcut(SignalBus signalBus)
        {
            _signalBus = signalBus;

            EssenceType = Layer;

            signalBus.Fire(new EssenceServiceSignals.Register(this));
        }

    }
}
