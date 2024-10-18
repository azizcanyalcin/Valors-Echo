using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField] private float hoverAmplitude = 0.5f;
    [SerializeField] private float hoverSpeed = 2f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
