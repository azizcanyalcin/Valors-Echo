using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Skeleton skeleton) : base(enemy, stateMachine, animatorBoolName, skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        skeleton.SetVelocity(skeleton.moveSpeed * skeleton.facingDirection, rb.velocity.y);

        if (skeleton.IsWallDetected() || !skeleton.IsGroundDetected())
        {
            skeleton.Flip();
            skeleton.SetVelocityToZero();
            stateMachine.ChangeState(skeleton.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}