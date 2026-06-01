using UnityEngine;
using System.Collections;

namespace Terraphibian
{
    public class HitStop : MonoBehaviour
    {
        public static HitStop Instance;

        void Awake()
        {
            Instance = this;
        }

        public void Stop(float duration)
        {
            StartCoroutine(DoHitStop(duration));
        }

        private IEnumerator DoHitStop(float duration)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
        }
    }
}