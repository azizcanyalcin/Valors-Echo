using System.Collections;
using UnityEngine;

public class DruidHealState : EnemyState
{
    Druid druid;
    Player player => PlayerManager.instance.player;
    int defaultArmor;
    int defaultMagicResistance;
    float healInterval = 0.25f;
    float lastHealTime;

    public DruidHealState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName)
    {
        this.druid = druid;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlayDelayedSFX(42, druid.transform, false, 0.3f);
        defaultArmor = druid.stats.armor.GetValue();
        defaultMagicResistance = druid.stats.magicResistance.GetValue();

        druid.stats.armor.SetValue(999);
        druid.stats.magicResistance.SetValue(999);

        // Initialize heal timer
        lastHealTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        druid.SetVelocityToZero();
        druid.CheckFlip();
        HealOverTime();
        if (triggerCalled)
            stateMachine.ChangeState(druid.battleState);
    }

    private void HealOverTime()
    {
        if (Time.time >= lastHealTime + healInterval)
        {
            Heal();
            lastHealTime = Time.time;
        }
    }
    private void Heal()
    {
        druid.stats.currentHealth += druid.stats.maxHealth.GetValue() / 95;
        druid.stats.TakeDamage(1); // quick fix for updating health ui
    }
    public override void Exit()
    {
        base.Exit();
        druid.stats.armor.SetValue(defaultArmor);
        druid.stats.magicResistance.SetValue(defaultMagicResistance);
        druid.lastTimeHealed = Time.time;
    }
}
