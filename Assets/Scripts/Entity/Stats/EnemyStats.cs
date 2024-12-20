using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDrop;
    public Stat soulsDropAmount;
    private HealthBarUI healthBarUI;

    [Header("Level")]
    [SerializeField] private int level = 1;
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;
    private void Awake()
    {
        if(EndGameManager.Instance)
            level = EndGameManager.Instance.levelUpAmount;
    }
    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        itemDrop = GetComponent<ItemDrop>();
        healthBarUI = GetComponentInChildren<HealthBarUI>();

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
    public void LevelUp(int levelAmount)
    {
        level = levelAmount;
    }
    protected override void Die()
    {
        base.Die();

        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        itemDrop.GenerateDrop();
        Destroy(healthBarUI.gameObject);
        Destroy(gameObject, 2f);
    }
}