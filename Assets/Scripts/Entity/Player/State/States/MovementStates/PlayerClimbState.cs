using System.Collections;
using UnityEngine;

public class PlayerClimbState : PlayerState
{
    public PlayerClimbState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();
        if (player.isPlayerNearLadder)
            rb.velocity = new Vector2(0, yInput * 5);
        if (!player.isPlayerNearLadder)
            player.stateMachine.ChangeState(player.idleState);
        if (Input.GetKeyDown(KeyCode.Space))
            player.stateMachine.ChangeState(player.jumpState);
        
    }
    private void CheckForGround()
    {
        if (player.IsGroundDetected())
            player.stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 3.5f;
    }

}
