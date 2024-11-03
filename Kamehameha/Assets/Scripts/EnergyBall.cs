using System;
using UnityEngine;

namespace Kamehameha
{
    public class EnergyBall : MonoBehaviour
    {
        public event Action TriggerEntered;
        public event Action TriggerExited;

        [field: SerializeReference]
        public GameObject Blast { get; private set; }

        [field: SerializeReference]
        public GameObject Ball { get; private set; }

        [field: SerializeReference]
        public GameObject Particles { get; private set; }

        [field: SerializeReference]
        public Transform ParentTransform { get; private set; }

        [field: SerializeReference]
        public ParticleSystem FirstStageEnd { get; private set; }


        [field: SerializeReference]
        public ParticleSystem SecondStageEnd { get; private set; }


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger entered");
            TriggerEntered?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExited?.Invoke();
        }
    }
}
