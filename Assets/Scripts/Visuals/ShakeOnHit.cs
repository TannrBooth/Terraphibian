using UnityEngine;
using System.Collections;

namespace Terraphibian
{
    public class ShakeOnHit : MonoBehaviour
    {
        [SerializeField] float duration = 0.15f;
        [SerializeField] float magnitude = 0.08f;

        Vector3 originalPosition;

        void Awake()
        {
            originalPosition = transform.localPosition;
        }

        public void Shake()
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine());
        }

        IEnumerator ShakeCoroutine()
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition =
                    originalPosition + new Vector3(x, y, 0);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = originalPosition;
        }
    }
}