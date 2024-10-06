using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassDeadState : EnemyState
{
    private Assassin assass;

    public AssassDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }

    public override void Enter()
    {
        base.Enter();
        assass.SetVelocityToZero();
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