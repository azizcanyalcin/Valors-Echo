using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.2f;
        player.SetVelocity(5 * -player.facingDirection, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(player.airState);
    }
    public override void Exit()
    {
        base.Exit();
    }

}
