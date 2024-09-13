using UnityEngine;

public class SlimeMoveState : SlimeGroundedState
{
    public SlimeMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Slime slime) : base(enemy, stateMachine, animatorBoolName, slime)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        slime.SetVelocity(slime.moveSpeed * slime.facingDirection, rb.velocity.y);

        if (slime.IsWallDetected() || !slime.IsGroundDetected())
        {
            slime.Flip();
            slime.SetVelocityToZero();
            stateMachine.ChangeState(slime.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}