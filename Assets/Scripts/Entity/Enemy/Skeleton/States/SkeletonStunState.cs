using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunState : EnemyState
{
    private Skeleton skeleton;
    public SkeletonStunState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Skeleton skeleton) : base(enemy, stateMachine, animatorBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        skeleton.fx.InvokeRepeating("RedBlink", 0, .1f);

        stateTimer = skeleton.stunDuration;
        rb.velocity = new Vector2(-1 * skeleton.facingDirection * skeleton.stunKnockBackPower.x, skeleton.stunKnockBackPower.y);
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(skeleton.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.fx.Invoke("CancelColorChange", 0);
    }
}