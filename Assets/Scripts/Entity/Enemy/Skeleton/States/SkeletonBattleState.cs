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

        if (IsPlayerDead())
        {
            stateMachine.ChangeState(skeleton.moveState);
        }
    }

    public override void Update()
    {
        base.Update();

        if (skeleton.IsPlayerDetected())
        {
            stateTimer = skeleton.battleTime;
            HandlePlayerInRange();
        }
        else if (ShouldReturnToIdle())
        {
            stateMachine.ChangeState(skeleton.idleState);
        }

        HandleMovement();
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

    private void HandlePlayerInRange()
    {
        if (skeleton.IsPlayerDetected().distance < skeleton.attackDistance && CanAttack())
        {
            stateMachine.ChangeState(skeleton.attackState);
        }
    }

    private bool ShouldReturnToIdle()
    {
        return stateTimer < 0 || Vector2.Distance(player.position, skeleton.transform.position) > 15;
    }

    private void HandleMovement()
    {
        float distanceToPlayerX = Mathf.Abs(player.position.x - skeleton.transform.position.x);

        // Determine whether the skeleton should flip
        bool shouldFlip = (player.position.x > skeleton.transform.position.x && moveDirection == -1) ||
                          (player.position.x < skeleton.transform.position.x && moveDirection == 1);

        // Update move direction only if the player is sufficiently far away OR if flipping is required
        if (distanceToPlayerX > 1.5f || shouldFlip)
        {
            moveDirection = (player.position.x > skeleton.transform.position.x) ? 1 : -1;
            skeleton.SetVelocity(skeleton.moveSpeed * moveDirection, rb.velocity.y);
        }
    }

    private bool IsPlayerDead()
    {
        return player.GetComponent<PlayerStats>().isDead;
    }
}
