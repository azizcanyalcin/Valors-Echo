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

        if (archer.IsPlayerDetected())
        {
            stateTimer = archer.battleTime;

            if (archer.IsPlayerDetected().distance < archer.safeDistance)
            {
                if (CanJump()) stateMachine.ChangeState(archer.jumpState);
            }

            if (archer.IsPlayerDetected().distance < archer.attackDistance && CanAttack())
                stateMachine.ChangeState(archer.attackState);
        }
        else if (stateTimer < 0 || Vector2.Distance(player.transform.position, archer.transform.position) > 15)
            stateMachine.ChangeState(archer.moveState);

        moveDirection = player.position.x > archer.transform.position.x ? 1 : -1;

        if(moveDirection != archer.facingDirection) archer.Flip();
        

    }
    private bool CanAttack()
    {
        if (Time.time >= archer.lastTimeAttacked + archer.attackCooldown)
        {
            archer.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
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