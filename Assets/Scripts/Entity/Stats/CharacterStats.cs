using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage
}

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength; // +1 damage point, +1 crit power
    public Stat agility; // +%1 evasion change, +%1 crit chance
    public Stat intelligence; // +1 magic damage, +3 magic resistance
    public Stat vitality; // + 3-5 hp, +3 damage resistance

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;


    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Elemental Damage Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    [Header("Status Effects")]
    public bool isBurning;     //Damage over time
    public bool isFrozen;      //Reduce armor by %20
    public bool isShocked; //Reduce accuracy by %20

    [SerializeField] private float debuffDuration = 2;
    private float burningDuration;
    private float burningFrequency = 0.5f;
    private float burningFrequencyDuration;
    private int burnDamage;

    private float freezeDuration;

    private float shockDuration;
    private int shockDamage;

    [SerializeField] private GameObject shockStrike;
    public int currentHealth;
    private bool isVulnerable;
    public bool isDead { get; private set; }
    public bool isImmune { get; private set; }
    public bool isAttacked;
    public System.Action onHealthChanged;
    private EntityFX fx;

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        critPower.SetValue(100);
        StartCoroutine(InitializeStats());
    }

    private IEnumerator InitializeStats()
    {
        yield return new WaitForSeconds(0f); // Wait for other components to initialize
        currentHealth = GetMaxHealthValue();
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    protected virtual void Update()
    {
        if (isBurning) UpdateBurningStatus();
        if (isFrozen) UpdateFreezingStatus();
        if (isShocked) UpdateShockingStatus();
            //Debug.Log(maxHealth.GetValue());
    }
    public virtual void TakeDamage(int damage)
    {
        if (isImmune) return;

        DecreaseHealth(damage);
        isAttacked = true;
        //GetComponent<Entity>().DamageImpact(); // Quick fix for player freeze
        fx.StartCoroutine("FlashFX");

    }
    public virtual void DealPhysicalDamage(CharacterStats target, float damageMultiplier)
    {
        if (CanTargetAvoidAttack(target) || target.isImmune) return;

        target.GetComponent<Entity>().SetupKnockBackDirection(transform);

        CreateHitFx(target.transform);

        target.TakeDamage(CalculatePhysicalDamage(target, damageMultiplier));
        DealElementalDamage(target);
    }
    public virtual void DealElementalDamage(CharacterStats target)
    {
        target.GetComponent<Entity>().SetupKnockBackDirection(transform);
        int totalElementalDamage = CalculateElementalDamage(target);
        target.TakeDamage(totalElementalDamage);


        ApplyDebuff(target);
    }
    protected int CalculatePhysicalDamage(CharacterStats target, float damageMultiplier)
    {
        int totalDamage = damage.GetValue() + strength.GetValue() + CalculateCritDamage() - target.armor.GetValue();
        totalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
        if (target.isFrozen) totalDamage += Mathf.RoundToInt(target.armor.GetValue() * 0.2f);

        return Mathf.Max(0, totalDamage);
    }
    private int CalculateCritDamage()
    {
        return CanCrit() ? Mathf.RoundToInt(damage.GetValue() * ((critPower.GetValue() + strength.GetValue()) * 0.01f)) : 0;
    }
    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        return Random.Range(0, 100) <= totalCritChance;
    }
    public virtual void OnEvasion()
    {
        AudioManager.instance.PlaySFX(58, transform, false);
    }
    protected bool CanTargetAvoidAttack(CharacterStats target)
    {
        int totalEvasion = target.evasion.GetValue() + target.agility.GetValue();
        if (isShocked) totalEvasion += 20;
        if (Random.Range(0, 100) <= totalEvasion)
        {
            target.OnEvasion();
            return true;
        }
        return false;
    }
    private int CalculateElementalDamage(CharacterStats target)
    {
        int totalElementalDamage = fireDamage.GetValue() + iceDamage.GetValue() + lightningDamage.GetValue() + intelligence.GetValue();
        totalElementalDamage -= target.magicResistance.GetValue() + (target.intelligence.GetValue() * 3);
        return Mathf.Max(0, totalElementalDamage);
    }
    public virtual void UpdateBurningStatus()
    {
        burningDuration -= Time.deltaTime;
        burningFrequencyDuration -= Time.deltaTime;

        if (burningDuration < 0) isBurning = false;
        if (burningFrequencyDuration < 0)
        {

            DecreaseHealth(burnDamage);
            burningFrequencyDuration = burningFrequency;
        }
    }
    public virtual void UpdateFreezingStatus()
    {
        freezeDuration -= Time.deltaTime;
        if (freezeDuration < 0) isFrozen = false;
    }
    public virtual void UpdateShockingStatus()
    {
        shockDuration -= Time.deltaTime;
        if (shockDuration < 0) isShocked = false;
    }

    public virtual void ApplyDebuff(CharacterStats target)
    {
        int totalDamage = fireDamage.GetValue() + iceDamage.GetValue() + lightningDamage.GetValue();

        if (totalDamage <= 0) return;


        float noEffectChance = Mathf.Clamp(0.8f - (intelligence.GetValue() * 0.02f), 0f, 0.8f);

        if (Random.Range(0f, 1f) < noEffectChance)
        {
            target.isBurning = false;
            target.isFrozen = false;
            target.isShocked = false;
            return;
        }

        float fireChance = (float)fireDamage.GetValue() / totalDamage;
        float iceChance = (float)iceDamage.GetValue() / totalDamage;
        float lightningChance = (float)lightningDamage.GetValue() / totalDamage;

        float totalChance = fireChance + iceChance + lightningChance;
        float randomValue = Random.Range(0f, totalChance);

        if (randomValue < fireChance)
        {
            ApplyBurningEffect(target);
        }
        else if (randomValue < fireChance + iceChance)
        {
            ApplyFrostEffect(target);
        }
        else
        {
            ApplyShockEffect(target);
        }
    }
    private void ApplyBurningEffect(CharacterStats target)
    {
        if (!target.isBurning && !target.isFrozen && !target.isShocked)
        {
            target.isBurning = true;
            target.fx.ApplyBurningEffect(debuffDuration);
            target.burningDuration = debuffDuration;
            target.burnDamage = Mathf.RoundToInt(fireDamage.GetValue() * 0.2f);
        }
        target.isFrozen = false;
        target.isShocked = false;
    }
    private void ApplyFrostEffect(CharacterStats target)
    {
        if (!target.isBurning && !target.isFrozen && !target.isShocked)
        {
            target.isBurning = false;
            target.isFrozen = true;
            target.freezeDuration = debuffDuration;
            target.GetComponent<Entity>().SlowEntity(.2f, debuffDuration);
            target.fx.ApplyFrostEffect(debuffDuration);

        }
        target.isShocked = false;
    }
    private void ApplyShockEffect(CharacterStats target)
    {
        if (!target.isBurning && !target.isFrozen)
        {
            if (!target.isShocked)
            {
                target.isBurning = false;
                target.isFrozen = false;
                target.isShocked = true;

                target.shockDuration = debuffDuration;
                target.shockDamage = Mathf.RoundToInt(lightningDamage.GetValue() * 0.2f);
                target.fx.ApplyShockedEffect(debuffDuration);
            }
            else
            {
                if (GetComponent<Enemy>() != null) return;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(target.transform.position, 25);
                float closestDistance = Mathf.Infinity;
                Transform closestEnemy = null;

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null && Vector2.Distance(target.transform.position, hit.transform.position) > 1)
                    {
                        float distanceToEnemy = Vector2.Distance(target.transform.position, hit.transform.position);
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            closestEnemy = hit.transform;
                        }
                    }
                    if (closestEnemy == null) closestEnemy = target.transform;
                }

                if (closestEnemy != null)
                {
                    GameObject newShockStrike = Instantiate(shockStrike, target.transform.position, Quaternion.identity);
                    newShockStrike.GetComponent<ShockStrikeController>().InitializeShockStrike(shockDamage, closestEnemy.GetComponent<CharacterStats>());
                }

            }
        }
    }
    public virtual void IncreaseHealth(int healthAmount)
    {
        currentHealth += healthAmount;
        if (currentHealth > maxHealth.GetValue()) currentHealth = maxHealth.GetValue();
        onHealthChanged?.Invoke();
    }
    public virtual void IncreaseStat(int modifier, float duration, Stat stat)
    {
        StartCoroutine(IncreaseStatCoroutine(modifier, duration, stat));
    }
    private IEnumerator IncreaseStatCoroutine(int modifier, float duration, Stat stat)
    {
        stat.AddModifier(modifier);
        yield return new WaitForSeconds(duration);
        stat.RemoveModifier(modifier);
    }
    public void ApplyVulnerable(int duration)
    {
        StartCoroutine(SetVulnerableCoroutine(duration));
    }
    private IEnumerator SetVulnerableCoroutine(int duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(duration);
        isVulnerable = false;
    }
    public virtual void DecreaseHealth(int damage)
    {
        if (isVulnerable) currentHealth -= Mathf.RoundToInt(damage * 1.1f);
        else currentHealth -= damage;

        // if(damage <= 0) fx.CreatePopUpText("Miss");
        // else fx.CreatePopUpText(damage.ToString());

        onHealthChanged?.Invoke();

        if (currentHealth <= 0 && !isDead) Die();
    }
    protected virtual void Die()
    {
        isDead = true;
    }
    public Stat GetStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return critChance;
            case StatType.critPower:
                return critPower;
            case StatType.maxHealth:
                return maxHealth;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicResistance:
                return magicResistance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightningDamage:
                return lightningDamage;
            default:
                Debug.LogError("Unknown StatType: " + statType);
                return null;
        }
    }
    public void KillEntity()
    {
        if (!isDead) Die();
    }
    public void SetImmunability(bool isImmune) => this.isImmune = isImmune;

    private void CreateHitFx(Transform target)
    {
        if (CanCrit()) fx.CreateCriticalHitFx(target);
        else fx.CreateHitFx(target);
    }
}