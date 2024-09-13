using UnityEngine;

public class ArcherJumpState : EnemyState
{
    
    private Archer archer;
    public ArcherJumpState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();
        if (!archer.IsGroundBehind() || archer.IsWallBehind())
            rb.velocity = new Vector2(archer.jump.x * archer.facingDirection, archer.jump.y);
        else
            rb.velocity = new Vector2(archer.jump.x * -archer.facingDirection, archer.jump.y);
    }

    public override void Update()
    {
        base.Update();

        archer.animator.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0 && archer.IsGroundDetected())
        {
            archer.SetVelocityToZero();
            stateMachine.ChangeState(archer.battleState);
        }
    }
}