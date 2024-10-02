using Cinemachine;
using UnityEngine;

public class CameraTargetChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform playerTarget; 
    [SerializeField] private Transform newTarget; 

    private void Start()
    {
        virtualCamera.Follow = playerTarget;
        virtualCamera.LookAt = playerTarget;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.Follow = newTarget;
            virtualCamera.LookAt = newTarget;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.Follow = playerTarget;
            virtualCamera.LookAt = playerTarget;
        }
    }
}
