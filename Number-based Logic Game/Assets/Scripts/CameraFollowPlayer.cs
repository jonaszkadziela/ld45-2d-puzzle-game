using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera followCamera;

    void Start()
    {
        followCamera = GetComponent<CinemachineVirtualCamera>();

        if (PlayerController.Instance)
        {
            followCamera.Follow = PlayerController.Instance.transform;
        }
    }

    void Update()
    {
        if (PlayerController.Instance && followCamera.Follow != PlayerController.Instance.transform)
        {
            followCamera.Follow = PlayerController.Instance.transform;
        }
    }
}
