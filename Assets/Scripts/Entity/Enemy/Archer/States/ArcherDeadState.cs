using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDeadState : EnemyState
{
    private Archer archer;

    public ArcherDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();

        archer.animator.SetBool(archer.lastAnimatorBoolName, true);
        archer.animator.speed = 0;
        enemy.capsuleCollider.enabled = false;

        stateTimer = .1f;
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}