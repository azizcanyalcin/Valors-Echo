using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Skeleton skeleton;
    protected Transform player;
    public SkeletonGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Skeleton skeleton) : base(enemy, stateMachine, animatorBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();
        if (skeleton.IsPlayerDetected() || Vector2.Distance(player.transform.position, skeleton.transform.position) < skeleton.agroDistance)
            stateMachine.ChangeState(skeleton.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}