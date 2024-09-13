using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        GameObject.Find("Canvas").GetComponent<UIManager>().SwitchToEndScreen();
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocityToZero();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
