using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDrop;
    public Stat soulsDropAmount;

    [Header("Level")]
    [SerializeField] private int level = 1;
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;
    protected override void Start()
    {
        soulsDropAmount.SetValue(100);
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        itemDrop = GetComponent<ItemDrop>();

    }
    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);


        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);


        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(soulsDropAmount);
    }
    private void Modify(Stat stat)
    {
        for (int i = 0; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;
            stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();
        
        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        itemDrop.GenerateDrop();

        Destroy(gameObject, 5f);
    }
}