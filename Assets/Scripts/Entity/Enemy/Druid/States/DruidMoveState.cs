using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruidMoveState : DruidGroundedState
{
    public DruidMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName, druid)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        druid.FlipToPlayer();
        druid.SetVelocity(druid.moveSpeed * druid.facingDirection, rb.velocity.y);

        if (druid.IsWallDetected() || !druid.IsGroundDetected())
        {
            druid.Flip();
            druid.SetVelocityToZero();
            stateMachine.ChangeState(druid.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}