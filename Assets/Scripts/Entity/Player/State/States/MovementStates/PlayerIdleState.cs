using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(0, 0);

    }
    public override void Update()
    {
        base.Update();
        rb.velocity = new Vector2(0, rb.velocity.y);
        //player.SetVelocityToZero();
        //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if (xInput == player.facingDirection && player.IsWallDetected())
            return;
        if (xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
    public override void Exit()
    {
        base.Exit();
    }

}
