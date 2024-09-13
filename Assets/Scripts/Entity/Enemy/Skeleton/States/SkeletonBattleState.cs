using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Skeleton skeleton;
    private int moveDirection;
    public SkeletonBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Skeleton skeleton) : base(enemy, stateMachine, animatorBoolName)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if(player.GetComponent<PlayerStats>().isDead) stateMachine.ChangeState(skeleton.moveState);
    }
    public override void Update()
    {
        base.Update();

        if (skeleton.IsPlayerDetected())
        {
            stateTimer = skeleton.battleTime;
            if (skeleton.IsPlayerDetected().distance < skeleton.attackDistance && CanAttack())
                stateMachine.ChangeState(skeleton.attackState);
        }
        else if (stateTimer < 0 || Vector2.Distance(player.transform.position, skeleton.transform.position) > 15)
            stateMachine.ChangeState(skeleton.idleState);

        if (player.position.x > skeleton.transform.position.x)
            moveDirection = 1;
        else
            moveDirection = -1;

        skeleton.SetVelocity(skeleton.moveSpeed * moveDirection, rb.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();

    }
    private bool CanAttack()
    {
        if (Time.time >= skeleton.lastTimeAttacked + skeleton.attackCooldown)
        {
            skeleton.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}