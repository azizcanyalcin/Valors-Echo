using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassBattleState : EnemyState
{
    private Transform player;
    private Assassin assass;
    private int moveDirection;

    public AssassBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(assass.moveState);
        }
    }

    public override void Update()
    {
        base.Update();
        float distanceToPlayerX = Mathf.Abs(player.position.x - assass.transform.position.x);
        if (CanCastUlti())
        {
            stateTimer = assass.battleTime;
            stateMachine.ChangeState(assass.ultimateAttackState);
        }
        else if (assass.IsPlayerDetected())
        {
            stateTimer = assass.battleTime;
            HandlePlayerInRange();
        }
        else if (ShouldReturnToIdle())
        {
            stateMachine.ChangeState(assass.idleState);
            Debug.Log($"returned idle");
        }
        HandleMovement();
    }
    private bool ShouldReturnToIdle()
    {
        return stateTimer < 0;
    }

    private void HandlePlayerInRange()
    {
        if (assass.IsPlayerDetected().distance <= assass.attackDistance && CanAttack())
        {
            stateMachine.ChangeState(assass.attackState);
        }
    }

    private void HandleMovement()
    {
        if(CanJump())
        {
            stateMachine.ChangeState(assass.jumpState);
        }  
        float distanceToPlayerX = Mathf.Abs(player.position.x - assass.transform.position.x);

        // Determine whether the assass should flip
        bool shouldFlip = (player.position.x > assass.transform.position.x && moveDirection == -1) ||
                          (player.position.x < assass.transform.position.x && moveDirection == 1);

        // Update move direction only if the player is sufficiently far away OR if flipping is required
        if (distanceToPlayerX > assass.attackDistance - 0.1f || shouldFlip)
        {
            moveDirection = (player.position.x > assass.transform.position.x) ? 1 : -1;
            assass.SetVelocity(assass.moveSpeed * moveDirection, rb.velocity.y);
        }
    }

    private bool CanAttack()
    {
        return CheckCooldown(ref assass.lastTimeAttacked, assass.attackCooldown);
    }

    private bool CanCastUlti()
    {
        return CheckCooldown(ref assass.lastTimeUltimate, assass.ultimateAttackCooldown);
    }

    private bool CanJump()
    {
        return CheckCooldown(ref assass.lastTimeJumped, assass.jumpCooldown);
    }

    private bool CheckCooldown(ref float lastActionTime, float cooldown)
    {
        if (Time.time >= lastActionTime + cooldown)
        {
            lastActionTime = Time.time;
            return true;
        }
        return false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
