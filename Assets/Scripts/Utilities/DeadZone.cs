using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterStats stats = collision.GetComponent<CharacterStats>();
        if (stats) stats.KillEntity();
        else Destroy(collision.gameObject);
    }
}