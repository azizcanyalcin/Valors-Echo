using System.Collections;
using UnityEngine;

public class Priest : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = PlayerManager.instance.player;
        player.isPlayerActive = true;
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