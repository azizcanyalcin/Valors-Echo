using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] private Enemy boss;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial transform values
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Override the transform to keep it frozen
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>()) boss.isTriggered = true;
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>()) boss.isTriggered = false;
    }
}