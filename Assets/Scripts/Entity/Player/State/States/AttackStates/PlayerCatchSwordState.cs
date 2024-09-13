using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform swordTransform;
    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.fx.PlayDustFx();
        //QUICK NULL CHECK HERE. CHECK LATER!!
        if (player != null && player.sword != null && player.sword.transform != null)
        {
            swordTransform = player.sword.transform;
        }
        else
        {
            return;
        }

        float swordDirection = swordTransform.position.x - player.transform.position.x;

        rb.velocity = new Vector2(2 * -swordDirection, rb.velocity.y);

    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);

    }
}