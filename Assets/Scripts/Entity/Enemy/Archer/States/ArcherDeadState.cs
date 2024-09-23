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
        archer.SetVelocityToZero();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}