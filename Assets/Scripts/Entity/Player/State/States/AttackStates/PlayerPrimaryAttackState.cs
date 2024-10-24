using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.animator.SetInteger("ComboCounter", comboCounter);

        float attackDirection = player.facingDirection;
        if (xInput != 0)
        {
            attackDirection = xInput;
        }

        //player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);
        // player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.rb.velocity.y);

        stateTimer = .1f;
    }
    public override void Update()
    {
        base.Update();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Mathf.Sign(mousePosition.x - player.transform.position.x) != player.facingDirection)
            player.Flip();
        if (stateTimer < 0 && player.IsGroundDetected())
            player.SetVelocityToZero();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .17f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }
}
