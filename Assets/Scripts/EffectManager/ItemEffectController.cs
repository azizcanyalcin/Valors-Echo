using UnityEngine;

public class ItemEffectController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemy = collision.GetComponent<EnemyStats>();
            player.DealElementalDamage(enemy);
        }
    }
}