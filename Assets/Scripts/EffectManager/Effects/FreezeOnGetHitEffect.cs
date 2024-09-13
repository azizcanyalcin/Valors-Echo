using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Effect", menuName = "Data/Item Effect/Freeze On Get Hit Effect")]

public class FreezeOnGetHitEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform transform)
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();


        if(!Inventory.instance.CanUseArmorEffect() || player.currentHealth > player.maxHealth.GetValue() * 0.1f) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.UseFreezeTimeCouroutine(duration);
        }
    }
}