using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Skeleton skeleton) : base(enemy, stateMachine, animatorBoolName, skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = skeleton.idleTime;
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(skeleton.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        

    }
}