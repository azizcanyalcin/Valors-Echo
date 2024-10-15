using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidDeadState : EnemyState
{
    Druid druid;
    public DruidDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName)
    {
        this.druid = druid;
    }

    public override void Enter()
    {
        base.Enter();
        druid.SetVelocityToZero();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}