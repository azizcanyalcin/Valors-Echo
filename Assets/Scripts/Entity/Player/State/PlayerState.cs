using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    protected float xInput;
    protected float yInput;
    protected bool triggerCalled;
    protected float stateTimer;

    private string animatorBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animatorBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animatorBoolName = animatorBoolName;
    }
    public virtual void Enter()
    {
        player.animator.SetBool(animatorBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.animator.SetFloat("yVelocity", rb.velocity.y);
    }
    public virtual void Exit()
    {
        player.animator.SetBool(animatorBoolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public void TurnPlayerWithMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Mathf.Sign(mousePosition.x - player.transform.position.x) != player.facingDirection)
            player.Flip();
    }
}
