using UnityEngine;

public class EyeMoveState : EnemyState
{
    private Transform player;
    private Eye eye;

    public EyeMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName,Eye eye) : base(enemy, stateMachine, animatorBoolName)
    {
        this.eye = eye;
    }

    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        eye.SetVelocity(eye.moveSpeed * eye.facingDirection, rb.velocity.y);
        if(eye.DistanceToPlayer() <= eye.attackCheckRadius) stateMachine.ChangeState(eye.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}