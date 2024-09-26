using System.Collections;
using UnityEngine;

public class ArcherDefendState : EnemyState
{
    Archer archer;
    Player player => PlayerManager.instance.player;
    int defaultArmor;
    int defaultMagicResistance;
    float healInterval = 1f; // Heal every second
    float lastHealTime;

    public ArcherDefendState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();
        defaultArmor = archer.stats.armor.GetValue();
        defaultMagicResistance = archer.stats.magicResistance.GetValue();

        archer.stats.armor.SetValue(999);
        archer.stats.magicResistance.SetValue(999);

        // Initialize heal timer
        lastHealTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        archer.SetVelocityToZero();
        archer.CheckFlip();
        HealOverTime();
        if (triggerCalled)
            stateMachine.ChangeState(archer.battleState);
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
        archer.stats.currentHealth += archer.stats.maxHealth.GetValue() / 20;
        archer.stats.TakeDamage(1); // quick fix for updating health ui
    }
    public override void Exit()
    {
        base.Exit();
        archer.stats.armor.SetValue(defaultArmor);
        archer.stats.magicResistance.SetValue(defaultMagicResistance);
        archer.lastTimeDefend = Time.time;
    }
}
