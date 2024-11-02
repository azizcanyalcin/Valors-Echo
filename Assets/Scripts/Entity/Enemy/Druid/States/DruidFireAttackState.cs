using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidFireAttackState : EnemyState
{
    private Druid druid;

    public DruidFireAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName)
    {
        this.druid = druid;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(50, druid.transform, true);
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
        druid.lastTimeAttacked = Time.time;
    }
}