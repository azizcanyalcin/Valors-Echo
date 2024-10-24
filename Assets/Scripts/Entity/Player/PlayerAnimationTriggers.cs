using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        player.fx.ScreenShake(player.fx.shakePowerHit);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.TryGetComponent<EnemyStats>(out var enemy)) player.stats.DealPhysicalDamage(enemy, 1);
                TriggerWeaponEffect(enemy);
                AudioManager.instance.PlaySFX(1, player.transform, false);
            }
        }
        AudioManager.instance.PlaySFX(2, player.transform, false);
    }

    public void TriggerWeaponEffect(EnemyStats enemy)
    {
        Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
        if (weaponData != null) weaponData.Effect(enemy.transform);
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
