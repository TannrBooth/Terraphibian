using Unity.Cinemachine;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtualCamera.Follow = Character.Instance.transform;

    }

    // Update is called once per frame
    void Update()
    {

    }
}

