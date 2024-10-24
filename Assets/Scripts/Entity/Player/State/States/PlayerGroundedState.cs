using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackHole.isBlackHoleUnlocked && player.skill.blackHole.cooldownTimer < 0)
            stateMachine.ChangeState(player.blackHoleState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);

        if (!player.IsGroundDetected() && player.coyoteTimer < 0)
            stateMachine.ChangeState(player.airState);

        if (player.jumpBufferTimer > 0 && player.coyoteTimer > 0)
        {
            stateMachine.ChangeState(player.jumpState);
            player.jumpBufferTimer = 0;
        }

        if (Input.GetKeyUp(KeyCode.Space) && player.rb.velocity.y > 0)
            player.coyoteTimer = 0f;

        if (Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.parry.parryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);
    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
