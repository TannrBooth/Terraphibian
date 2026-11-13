using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

namespace Terraphibian
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;
        public CinemachineCamera virtualCamera;

        private CinemachineBasicMultiChannelPerlin noise;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake(float intensity, float duration)
        {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        }

        private IEnumerator ShakeCoroutine(float intensity, float duration)
        {
            noise.AmplitudeGain = intensity;
            yield return new WaitForSeconds(duration);
            noise.AmplitudeGain = 0;
        }

    }
}
