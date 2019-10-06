using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera followCamera;

    void Start()
    {
        followCamera = GetComponent<CinemachineVirtualCamera>();
        followCamera.Follow = PlayerController.Instance.transform;
    }
}
