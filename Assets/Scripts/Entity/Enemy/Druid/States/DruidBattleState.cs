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

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(druid.moveState);
            return;
        }
    }

    public override void Update()
    {
        base.Update();

        druid.FlipToPlayer();
        HandleStateTransitions();

        if (stateMachine.currentState == this)
        {
            CalculateDirection();
            Move();
        }
    }

    private void HandleStateTransitions()
    {
        stateTimer = druid.battleTime;
        if (CanRootAttack())
        {
            stateMachine.ChangeState(druid.rootAttackState);
            return;
        }

        if (CanAshThrow())
        {
            stateMachine.ChangeState(druid.ashThrowState);
            return;
        }

        if (druid.stats.currentHealth <= druid.stats.maxHealth.GetValue() / 2 && CanHeal())
        {
            stateMachine.ChangeState(druid.healState);
            return;
        }

        if (druid.IsPlayerDetected().distance < druid.attackDistance && CanFireAttack() && druid.IsPlayerDetected().distance != 0)
        {
            Debug.Log($"Druid is player detected log: " + druid.IsPlayerDetected().distance);
            Debug.Log($"Druid is attack distance log: " + druid.attackDistance);
            stateMachine.ChangeState(druid.fireAttackState);
            return;
        }
    }

    private void Move()
    {
        if (druid.IsPlayerDetected().distance < druid.attackDistance - 0.25f) return;
        druid.SetVelocity(druid.moveSpeed * moveDirection, rb.velocity.y);
    }

    private void CalculateDirection()
    {
        moveDirection = (player.position.x > druid.transform.position.x) ? 1 : -1;
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
    private bool CanFireAttack() => CanPerform(ref druid.lastTimeFireAttacked, druid.fireAttackCooldown);
    private bool CanWalkingFire() => CanPerform(ref druid.lastTimeWalkingFired, druid.walkingFireCooldown);
    private bool CanRootAttack() => CanPerform(ref druid.lastTimeRootAttacked, druid.rootAttackCooldown);
    private bool CanAshThrow() => CanPerform(ref druid.lastTimeAshThrowed, druid.ashThrowCooldown);
    private bool CanHeal() => CanPerform(ref druid.lastTimeHealed, druid.healCooldown);
    #endregion

    public override void Exit()
    {
        base.Exit();
    }
}
