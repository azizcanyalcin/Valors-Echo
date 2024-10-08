using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private Skeleton skeleton;
    public SkeletonDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Skeleton skeleton) : base(enemy, stateMachine, animatorBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.SetVelocityToZero();
        //skeleton.animator.SetBool(skeleton.lastAnimatorBoolName, true);
        //skeleton.animator.speed = 0;
        //enemy.capsuleCollider.enabled = false;

        //stateTimer = .1f;
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            //rb.velocity = new Vector2(0, 10);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}