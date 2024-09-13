using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStunState : EnemyState
{
    private Archer archer;

    public ArcherStunState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();

        archer.fx.InvokeRepeating("RedBlink", 0, .1f);

        stateTimer = archer.stunDuration;
        rb.velocity = new Vector2(-1 * archer.facingDirection * archer.stunKnockBackPower.x, archer.stunKnockBackPower.y);
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(archer.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        archer.fx.Invoke("CancelColorChange", 0);
    }
}