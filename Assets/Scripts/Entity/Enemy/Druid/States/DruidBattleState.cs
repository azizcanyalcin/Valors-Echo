using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class DruidBattleState : EnemyState
{
    private Transform player;
    private Druid druid;
    private int moveDirection;

    public DruidBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName)
    {
        this.druid = druid;
    }
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead) stateMachine.ChangeState(druid.moveState);
    }
    public override void Update()
    {
        base.Update();

        if (CanRootAttack())
            stateMachine.ChangeState(druid.rootAttackState);
        if (CanAshThrow())
            stateMachine.ChangeState(druid.ashThrowState);
        if (druid.stats.currentHealth <= druid.stats.maxHealth.GetValue() / 50 && CanHeal())
            stateMachine.ChangeState(druid.healState);
        if (druid.IsPlayerDetected().distance < druid.attackDistance && CanFireAttack())
        {
            stateTimer = druid.battleTime;
            stateMachine.ChangeState(druid.fireAttackState);
        }
        else
        {
            CalculateDirection();
            Move();
        }
    }
    private void Move()
    {
        druid.SetVelocity(druid.moveSpeed * moveDirection, rb.velocity.y);
    }
    private void CalculateDirection()
    {
        if (player.position.x > druid.transform.position.x)
            moveDirection = 1;
        else
            moveDirection = -1;
    }
    #region CooldownLogic
    private bool CanPerform(ref float lastTimeAttacked, float attackCooldown)
    {
        if (Time.time >= lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
    private bool CanFireAttack()
    {
        return CanPerform(ref druid.lastTimeFireAttacked, druid.fireAttackCooldown);
    }
    private bool CanWalkingFire()
    {
        return CanPerform(ref druid.lastTimeWalkingFired, druid.walkingFireCooldown);
    }
    private bool CanRootAttack()
    {
        return CanPerform(ref druid.lastTimeRootAttacked, druid.rootAttackCooldown);
    }
    private bool CanAshThrow()
    {
        return CanPerform(ref druid.lastTimeAshThrowed, druid.ashThrowCooldown);
    }
    private bool CanHeal()
    {
        return CanPerform(ref druid.lastTimeHealed, druid.healCooldown);
    }
    #endregion
    public override void Exit()
    {
        base.Exit();

    }
}