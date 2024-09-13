using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDash();

        stateTimer = player.dashDuration;

        player.stats.SetImmunability(true);
    }
    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        player.SetVelocity(player.dashSpeed * player.dashDirection, rb.velocity.y);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
        player.fx.CreateCloneFlashImage();
    }
    public override void Exit()
    {
        base.Exit();
        player.skill.dash.CloneOnArrival();
        player.SetVelocity(0, rb.velocity.y);
        player.stats.SetImmunability(false);

    }

}
