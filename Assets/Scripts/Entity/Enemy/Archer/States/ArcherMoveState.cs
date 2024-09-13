using UnityEngine;

public class ArcherMoveState : EnemyState
{
    private Transform player;
    private Archer archer;
    public ArcherMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        archer.SetVelocity(archer.moveSpeed * archer.facingDirection, rb.velocity.y);

        if (archer.IsWallDetected() || !archer.IsGroundDetected())
        {
            archer.Flip();
            archer.SetVelocityToZero();
            stateMachine.ChangeState(archer.idleState);
        }
        if(archer.IsPlayerDetected()) stateMachine.ChangeState(archer.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}