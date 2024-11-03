using System;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;

namespace Kamehameha
{
    public class GestureCombiner : MonoBehaviour
    {
        public event Action GestureDetected;
        public event Action GestureEnded;

        [SerializeField]
        private StaticHandGesture leftHandGesture;

        [SerializeField]
        private StaticHandGesture rightHandGesture;

        private bool _isRightGestureDetected;
        private bool _isLeftGestureDetected;

        private bool _bothGesturesDetected;

        private void Start()
        {
            leftHandGesture.gesturePerformed.AddListener(LeftGestureDetected);
            leftHandGesture.gestureEnded.AddListener(LeftGestureEnded);

            rightHandGesture.gesturePerformed.AddListener(RightGestureDetected);
            rightHandGesture.gestureEnded.AddListener(RightGestureEnded);
        }

        private void OnDestroy()
        {
            if (leftHandGesture)
            {
                leftHandGesture.gesturePerformed.RemoveListener(LeftGestureDetected);
                leftHandGesture.gestureEnded.RemoveListener(LeftGestureEnded);
            }

            if (rightHandGesture)
            {
                rightHandGesture.gesturePerformed.RemoveListener(RightGestureDetected);
                rightHandGesture.gestureEnded.RemoveListener(RightGestureEnded);
            }
        }

        private void LeftGestureDetected()
        {
            _isLeftGestureDetected = true;
            EvaluateGestures();
        }

        private void LeftGestureEnded()
        {
            _isLeftGestureDetected = false;
            EvaluateGestures();
        }

        private void RightGestureDetected()
        {
            _isRightGestureDetected = true;
            EvaluateGestures();
        }

        private void RightGestureEnded()
        {
            _isRightGestureDetected = false;
            EvaluateGestures();
        }

        private void EvaluateGestures()
        {
            if (_isLeftGestureDetected && _isRightGestureDetected)
            {
                if (!_bothGesturesDetected)
                {
                    GestureDetected?.Invoke();
                    _bothGesturesDetected = true;
                }
            }
            else
            {
                if (_bothGesturesDetected)
                {
                    GestureEnded?.Invoke();
                    _bothGesturesDetected = false;
                }
            }
        }
    }
}
