using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    private int startingHealth;
    private int maxHealthModifiers;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
        StartCoroutine(InitializeHealth());

    }
    private IEnumerator InitializeHealth()
    {
        yield return new WaitForSeconds(0.5f);
        currentHealth = GetMaxHealthValue();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
    public override void DecreaseHealth(int damage)
    {
        base.DecreaseHealth(damage);
        if (damage > maxHealth.GetValue() * .3f)
        {
            AudioManager.instance.PlaySFX(35, transform, false);
            player.SetupKnockBackPower(new Vector2(8, 10));
            player.fx.ScreenShake(player.fx.shakePowerHighDamage);
        }
        else
        {
            AudioManager.instance.PlaySFX(Random.Range(31, 34), transform, false);
        }

        Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor) currentArmor.Effect(player.transform);
    }
    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }
    public void CloneDamage(CharacterStats target, float damageMultiplier)
    {
        DealPhysicalDamage(target, damageMultiplier);
    }
    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
}