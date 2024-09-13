using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Skeleton skeleton;
    public SkeletonAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Skeleton skeleton) : base(enemy, stateMachine, animatorBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        skeleton.SetVelocityToZero();
        if (triggerCalled)
            stateMachine.ChangeState(skeleton.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        skeleton.lastTimeAttacked = Time.time;
    }
}