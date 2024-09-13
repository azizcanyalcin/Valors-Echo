using UnityEngine;

public class BodMoveState : EnemyState
{
    Bod bod;
    public BodMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Bod bod) : base(enemy, stateMachine, animatorBoolName)
    {
        this.bod = bod;
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        bod.SetVelocity(bod.moveSpeed * bod.facingDirection, rb.velocity.y);

        if (bod.IsWallDetected() || !bod.IsGroundDetected())
        {
            bod.Flip();
            bod.SetVelocityToZero();
            stateMachine.ChangeState(bod.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}