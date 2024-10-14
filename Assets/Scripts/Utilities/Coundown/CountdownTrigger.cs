using UnityEngine;

public class CountdownTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            CountdownManager.instance.StartCountdown();
    }
}