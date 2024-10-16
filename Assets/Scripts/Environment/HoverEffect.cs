using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField] private float hoverAmplitude = 0.5f; // The height the object will oscillate
    [SerializeField] private float hoverSpeed = 2f;       // The speed of the oscillation
    private Vector3 startPosition;                         // The initial position of the object

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;

        // Update the object's position (only the Y value changes)
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
