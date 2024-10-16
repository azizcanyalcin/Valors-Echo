using System.Collections;
using UnityEngine;

public class DruidHealState : EnemyState
{
    Druid druid;
    int defaultArmor;
    int defaultMagicResistance;

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
    }

    public override void Update()
    {
        base.Update();

        druid.SetVelocityToZero();
        druid.FlipToPlayer();
        if (triggerCalled)
            stateMachine.ChangeState(druid.battleState);
    }
    public override void Exit()
    {
        base.Exit();
        druid.stats.armor.SetValue(defaultArmor);
        druid.stats.magicResistance.SetValue(defaultMagicResistance);
        druid.lastTimeHealed = Time.time;
    }
}
