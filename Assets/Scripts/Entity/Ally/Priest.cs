using System.Collections;
using UnityEngine;

public class Priest : MonoBehaviour
{
    Player player;
    CapsuleCollider2D priestCollider;
    private void Start()
    {
        player = PlayerManager.instance.player;
        player.isPlayerActive = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Destroyer"))
        {
            player.isPlayerActive = true;
            Destroy(gameObject);
        }
    }
}