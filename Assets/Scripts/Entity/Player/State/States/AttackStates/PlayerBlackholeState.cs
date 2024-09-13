using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool isSkillUsed;
    private float initialGravity;
    public PlayerBlackHoleState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        
        initialGravity = rb.gravityScale;

        isSkillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }
        else
        {
            rb.velocity = new Vector2(0, -.1f);

            if(!isSkillUsed && player.skill.blackHole.CanUseSkill())
            {
                isSkillUsed = true;
            }    
        }

        if(player.skill.blackHole.IsBlackHoleFinished())
            stateMachine.ChangeState(player.airState);

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Exit()
    {
        base.Exit();
        
        rb.gravityScale = initialGravity;
        player.fx.MakeTransparent(false);
    }
}