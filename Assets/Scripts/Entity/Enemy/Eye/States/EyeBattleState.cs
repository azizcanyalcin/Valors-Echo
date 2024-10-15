using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EyeBattleState : EnemyState
{
    private Transform player;
    private Eye eye;
    private int moveDirection;

    public EyeBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Eye eye) : base(enemy, stateMachine, animatorBoolName)
    {
        this.eye = eye;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead) stateMachine.ChangeState(eye.moveState);
    }
    public override void Update()
    {
        base.Update();
        if (eye.DistanceToPlayer() <= eye.attackCheckRadius)
        {
            stateTimer = eye.battleTime;
            if (CanAttack())
                stateMachine.ChangeState(eye.attackState);
            else
                stateMachine.ChangeState(eye.moveState);
        }
        else
        {
            CalculateDirection();
            Move();
        }
    }
    private void Move()
    {
        eye.SetVelocity(eye.moveSpeed * moveDirection, rb.velocity.y);
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
    private void CalculateDirection()
    {
        if (player.position.x > eye.transform.position.x)
            moveDirection = 1;
        else
            moveDirection = -1;
    }
    private bool CanAttack()
    {
        return CanPerform(ref eye.lastTimeAttacked, eye.attackCooldown);
    }
    public override void Exit()
    {
        base.Exit();

    }
}