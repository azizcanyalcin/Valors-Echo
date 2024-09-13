using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] private Enemy boss;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>()) boss.isTriggered = true;
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Player>()) boss.isTriggered = false;
    }
}