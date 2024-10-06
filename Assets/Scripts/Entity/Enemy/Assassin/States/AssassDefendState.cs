using System.Collections;
using UnityEngine;

public class AssassDefendState : EnemyState
{
    Assassin assass;
    Player player => PlayerManager.instance.player;
    int defaultArmor;
    int defaultMagicResistance;
    float healInterval = 0.25f;
    float lastHealTime;

    public AssassDefendState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlayDelayedSFX(42, assass.transform, false, 0.3f);
        defaultArmor = assass.stats.armor.GetValue();
        defaultMagicResistance = assass.stats.magicResistance.GetValue();

        assass.stats.armor.SetValue(999);
        assass.stats.magicResistance.SetValue(999);

        // Initialize heal timer
        lastHealTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        assass.SetVelocityToZero();
        assass.CheckFlip();
        HealOverTime();
        if (triggerCalled)
            stateMachine.ChangeState(assass.battleState);
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
        assass.stats.currentHealth += assass.stats.maxHealth.GetValue() / 95;
        assass.stats.TakeDamage(1); // quick fix for updating health ui
    }
    public override void Exit()
    {
        base.Exit();
        assass.stats.armor.SetValue(defaultArmor);
        assass.stats.magicResistance.SetValue(defaultMagicResistance);
        assass.lastTimeDefend = Time.time;
    }
}
