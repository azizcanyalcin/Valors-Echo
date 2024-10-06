using UnityEngine;

public class AssassMoveState : EnemyState
{
    private Transform player;
    private Assassin assass;
    public AssassMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        assass.SetVelocity(assass.moveSpeed * assass.facingDirection, rb.velocity.y);

        if (assass.IsWallDetected() || !assass.IsGroundDetected())
        {
            assass.Flip();
            assass.SetVelocityToZero();
            stateMachine.ChangeState(assass.idleState);
        }
        if(assass.IsPlayerDetected()) stateMachine.ChangeState(assass.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}