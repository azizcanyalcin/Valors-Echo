using Cinemachine;
using UnityEngine;

public class CameraSizeChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomSpeed = 2f;  
    [SerializeField] private float targetZoom = 8f;  
    private float defaultOrthoSize;
    private float targetOrthoSize;  

    private void Start()
    {
        defaultOrthoSize = virtualCamera.m_Lens.OrthographicSize;
        targetOrthoSize = defaultOrthoSize;
    }

    private void Update()
    {
        ChangeOrtho();
    }

    private void ChangeOrtho()
    {
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                    virtualCamera.m_Lens.OrthographicSize,
                    targetOrthoSize,
                    Time.deltaTime * zoomSpeed
                );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetOrthoSize = targetZoom;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetOrthoSize = defaultOrthoSize; 
        }
    }

    
}
