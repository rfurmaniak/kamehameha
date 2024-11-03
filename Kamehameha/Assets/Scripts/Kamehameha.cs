using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kamehameha
{
    public class Kamehameha : MonoBehaviour
    {
        [SerializeField]
        private GestureCombiner firstStageDetector;

        [SerializeField]
        private GestureCombiner thirdStageDetector;

        [SerializeField]
        private EnergyBall energyBall;

        [SerializeField]
        private InputActionReference leftHandPosition;

        [SerializeField]
        private InputActionReference rightHandPosition;

        private LerpController _energyBallLerpFirstStage;
        private LerpController _particlesLerpFirstStage;
        private LerpController _energyBallLerpSecondStage;
        private bool _trackBall;
        private bool _firstStageDone;
        private bool _ballInTrigger;

        private void Start()
        {
            firstStageDetector.GestureDetected += FirstStageDetected;
            firstStageDetector.GestureEnded += FirstStageDisrupted;

            _trackBall = true;
        }

        private void Update()
        {
            if (_trackBall)
            {
                var leftHand = leftHandPosition.action.ReadValue<Vector3>();
                var rightHand = rightHandPosition.action.ReadValue<Vector3>();

                energyBall.ParentTransform.position = (leftHand + rightHand) / 2;
            }
        }

        private void OnDestroy()
        {
            if (firstStageDetector)
            {
                firstStageDetector.GestureDetected -= FirstStageDetected;
                firstStageDetector.GestureEnded -= FirstStageDisrupted;
            }
            if (energyBall)
            {
                energyBall.TriggerEntered -= OnTriggerEntered;
                energyBall.TriggerExited -= OnTriggerExited;
            }
            if (thirdStageDetector)
            {
               thirdStageDetector.GestureDetected -= OnLastGestureDetected;
            }
        }

        private async void FirstStageDetected()
        {
            //First time the gesture is detected
            if (!_energyBallLerpFirstStage)
            {
                _energyBallLerpFirstStage = energyBall.Ball.AddComponent<LerpController>();
                _particlesLerpFirstStage = energyBall.Particles.AddComponent<LerpController>();
                _particlesLerpFirstStage.LerpScale(Vector3.zero, Vector3.one * 0.2f, 2.0f).Forget();
                await _energyBallLerpFirstStage.LerpScale(Vector3.zero, Vector3.one * 0.25f, 2.0f);
                FinishFirstStage();
            }
            //We already created it, now we're resuming
            _energyBallLerpFirstStage.ResumeLerp();
            _particlesLerpFirstStage.ResumeLerp();
        }

        private void FirstStageDisrupted()
        {
            if (!_energyBallLerpFirstStage)
                return;
            _energyBallLerpFirstStage.PauseLerp();
            _particlesLerpFirstStage.PauseLerp();
        }

        private void FinishFirstStage()
        {
            Debug.Log("First stage done");
            _firstStageDone = true;    
            Destroy(_energyBallLerpFirstStage);
            Destroy(_particlesLerpFirstStage);
            energyBall.FirstStageEnd.Play();
            firstStageDetector.GestureDetected -= FirstStageDetected;
            firstStageDetector.GestureEnded -= FirstStageDisrupted;
            InitializeSecondStage();
        }

        private void InitializeSecondStage()
        {
            energyBall.TriggerEntered += OnTriggerEntered;
            energyBall.TriggerExited += OnTriggerExited;
        }

        private void OnTriggerExited()
        {
            if (!_energyBallLerpSecondStage)
                return;
            _ballInTrigger = false;
            _energyBallLerpSecondStage.PauseLerp();
        }

        private async void OnTriggerEntered()
        {
            _ballInTrigger = true;

            //First time the gesture is detected
            if (!_energyBallLerpSecondStage)
            {
                _energyBallLerpSecondStage = energyBall.Particles.AddComponent<LerpController>();
                await _energyBallLerpSecondStage.LerpScale(Vector3.one * 0.2f, Vector3.one * 1.5f, 4.0f);
                FinishSecondStage();
            }
            //We already created it, now we're resuming
            _energyBallLerpSecondStage.ResumeLerp();
        }

        private void FinishSecondStage()
        {
            Debug.Log("Second stage done");
            Destroy(_energyBallLerpSecondStage);
            energyBall.SecondStageEnd.Play();
            thirdStageDetector.GestureDetected += OnLastGestureDetected;
        }

        private void OnLastGestureDetected()
        {
            energyBall.Ball.SetActive(false);
            energyBall.Blast.SetActive(true);
            thirdStageDetector.GestureDetected -= OnLastGestureDetected;
        }
    }
}
