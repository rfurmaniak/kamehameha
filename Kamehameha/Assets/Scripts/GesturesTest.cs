using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightFencing
{
    public class GesturesTest : MonoBehaviour
    {
        public void PrintText()
        {
            Debug.Log("Gesture recognized");
        }

        public void PrintEndText()
        {
            Debug.Log("Gesture ended");
        }
    }
}
