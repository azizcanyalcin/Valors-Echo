using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private Slime slime;
    private Transform player;
    private int moveDirection;
    public SlimeBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Slime slime) : base(enemy, stateMachine, animatorBoolName)
    {
        this.slime = slime;
    }
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead) stateMachine.ChangeState(slime.moveState);
    }
    public override void Update()
    {
        base.Update();

        if (slime.IsPlayerDetected())
        {
            stateTimer = slime.battleTime;
            if (slime.IsPlayerDetected().distance < slime.attackDistance && CanAttack())
                stateMachine.ChangeState(slime.attackState);
        }
        else if (stateTimer < 0 || Vector2.Distance(player.transform.position, slime.transform.position) > 15)
            stateMachine.ChangeState(slime.idleState);

        if (player.position.x > slime.transform.position.x)
            moveDirection = 1;
        else
            moveDirection = -1;

        if (slime.IsPlayerDetected() && slime.IsPlayerDetected().distance < slime.attackDistance - .5f) return;
       
        slime.SetVelocity(slime.moveSpeed * moveDirection, rb.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();

    }
    private bool CanAttack()
    {
        if (Time.time >= slime.lastTimeAttacked + slime.attackCooldown)
        {
            slime.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}