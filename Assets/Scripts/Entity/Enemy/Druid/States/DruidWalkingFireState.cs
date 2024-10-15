using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidWalkingFireState : EnemyState
{
    private Druid druid;

    public DruidWalkingFireState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName)
    {
        this.druid = druid;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(37, druid.transform, false);
    }
    public override void Update()
    {
        base.Update();

        druid.SetVelocityToZero();
        if (triggerCalled)
            stateMachine.ChangeState(druid.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        druid.lastTimeAttacked = Time.time;
    }
}