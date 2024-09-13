using UnityEngine;

public class BodBattleState : EnemyState
{
    Bod bod;
    Transform player;
    private int moveDirection;
    public BodBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Bod bod) : base(enemy, stateMachine, animatorBoolName)
    {
        this.bod = bod;
    }
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead) stateMachine.ChangeState(bod.teleportState);
    }
    public override void Update()
    {
        base.Update();


        if (bod.IsPlayerDetected())
        {
            stateTimer = bod.battleTime;

            if (bod.IsPlayerDetected().distance < bod.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(bod.attackState);
                else
                    stateMachine.ChangeState(bod.idleState);
            }
        }
        CalculateDirection();

        if (bod.IsPlayerDetected() && bod.IsPlayerDetected().distance < bod.attackDistance - .1f) return;

        Move();

    }

    private void Move()
    {
        bod.SetVelocity(bod.moveSpeed * moveDirection, rb.velocity.y);
    }

    private void CalculateDirection()
    {
        if (player.position.x > bod.transform.position.x)
            moveDirection = 1;
        else
            moveDirection = -1;
    }
    private bool CanAttack()
    {
        if (Time.time >= bod.lastTimeAttacked + bod.attackCooldown)
        {
            bod.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
    public override void Exit()
    {
        base.Exit();

    }
}