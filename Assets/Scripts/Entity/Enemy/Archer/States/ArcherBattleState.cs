using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Transform player;
    private Archer archer;
    private int moveDirection;

    public ArcherBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead) stateMachine.ChangeState(archer.moveState);
    }
    public override void Update()
    {
        base.Update();
        if (IsHealthBelowHalf() && CanDefend()) stateMachine.ChangeState(archer.defendState);
        if (archer.IsPlayerDetected())
        {
            stateTimer = archer.battleTime;
            float playerDistance = archer.IsPlayerDetected().distance;

            if (playerDistance < archer.safeDistance)
            {
                if (CanAttack2()) stateMachine.ChangeState(archer.attack2State);
                else if (CanJump()) stateMachine.ChangeState(archer.jumpState);
            }
            else if (playerDistance < archer.attackDistance)
            {
                if (CanUltimate()) stateMachine.ChangeState(archer.ultimateAttackState);
                else if (CanAttack3()) stateMachine.ChangeState(archer.attack3State);
                else if (CanAttack1()) stateMachine.ChangeState(archer.attackState);
            }
        }

        else if (stateTimer < 0 || Vector2.Distance(player.transform.position, archer.transform.position) > 15)
            stateMachine.ChangeState(archer.moveState);

        moveDirection = player.position.x > archer.transform.position.x ? 1 : -1;

        if (moveDirection != archer.facingDirection)
            archer.Flip();
    }

    private bool IsHealthBelowHalf()
    {
        return archer.stats.currentHealth <= archer.stats.maxHealth.GetValue() / 2;
    }

    private bool CanPerform(ref float lastTimeAttacked, float attackCooldown)
    {
        if (Time.time >= lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    private bool CanAttack1()
    {
        return CanPerform(ref archer.lastTimeAttacked, archer.attackCooldown);
    }

    private bool CanAttack2()
    {
        return CanPerform(ref archer.lastTimeAttack2, archer.attack2Cooldown);
    }

    private bool CanAttack3()
    {
        return CanPerform(ref archer.lastTimeAttack3, archer.attack3Cooldown);
    }
    private bool CanUltimate()
    {
        return CanPerform(ref archer.lastTimeUltimate, archer.ultimateAttackCooldown);
    }
    private bool CanDefend()
    {
        return CanPerform(ref archer.lastTimeDefend, archer.defendCooldown);
    }

    private bool CanJump()
    {
        //if(!archer.IsGroundBehind()) return false;
        if (Time.time >= archer.lastTimeJumped + archer.jumpCooldown)
        {
            archer.lastTimeJumped = Time.time;
            return true;
        }
        return false;
    }
    public override void Exit()
    {
        base.Exit();

    }
}