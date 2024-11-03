using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Kamehameha
{
    public class LerpController : MonoBehaviour
    {
        private bool isLerping = false;
        private bool isPaused = false;

        public async UniTask LerpScale( Vector3 startScale, Vector3 endScale, float duration)
        {
            if (isLerping)
                return; // Prevent starting another lerp if one is already running

            isLerping = true;
            var elapsedTime = 0f;

            while (elapsedTime < duration && isLerping)
            {
                if (!isPaused)
                {
                    elapsedTime += Time.deltaTime;
                    var t = Mathf.Clamp01(elapsedTime / duration);
                    transform.localScale = Vector3.Lerp(startScale, endScale, t);
                }
                await UniTask.Yield();
            }

            if (isLerping)
            {
                transform.localScale = endScale; // Ensure it completes exactly at the end scale
            }

            isLerping = false;
        }

        public void PauseLerp()
        {
            isPaused = true;
        }

        public void ResumeLerp()
        {
            isPaused = false;
        }

        public void StopLerp()
        {
            isLerping = false;
        }
    }
}