using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Potion
}

[CreateAssetMenu(fileName = "New Item", menuName = "Data/Equipment")]
public class Equipment : Item
{

    [Header("Unique Effecs")]
    public float itemCooldown;
    public EquipmentType equipmentType;
    public ItemEffect[] itemEffects;


    [Header("Major Stats")]
    public int strength; // +1 damage point, +1 crit power
    public int agility; // +%1 evasion change, +%1 crit chance
    public int intelligence; // +1 magic damage, +3 magic resistance
    public int vitality; // + 3-5 hp, +3 damage resistance

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defensive Stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Elemental Damage Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft Requirements")]
    public List<InventoryItem> craftMaterials;

    private int descriptionLength;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }
    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public void Effect(Transform transform)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(transform);
        }
    }

    private void SetDescription(int value, string name)
    {
        if (value != 0)
        {
            if (sb.Length > 0) sb.AppendLine();
            if (value > 0) sb.Append("+ " + value + " " + name);
            descriptionLength++;
        }
    }
    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;
        SetDescription(strength, "Strength");
        SetDescription(agility, "Agility");
        SetDescription(intelligence, "Intelligence");
        SetDescription(vitality, "Vitality");
        SetDescription(damage, "Damage");
        SetDescription(critChance, "Critical Chance");
        SetDescription(critPower, "Critical Power");
        SetDescription(maxHealth, "Maximum Health");
        SetDescription(armor, "Armor");
        SetDescription(evasion, "Evasion");
        SetDescription(magicResistance, "Magic Resistance");
        SetDescription(fireDamage, "Fire Damage");
        SetDescription(iceDamage, "Ice Damage");
        SetDescription(lightningDamage, "Lightning Damage");


        foreach (var itemEffect in itemEffects)
        {
            if (itemEffect.effectDescription.Length > 0)
            {   
                sb.AppendLine("");
                sb.AppendLine("Unique: " + itemEffect.effectDescription);
                descriptionLength++;
            }
        }

        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        return sb.ToString();
    }
}
