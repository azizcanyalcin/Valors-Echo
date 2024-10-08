using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDeadState : EnemyState
{
    private Eye eye;

    public EyeDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Eye eye) : base(enemy, stateMachine, animatorBoolName)
    {
        this.eye = eye;
    }

    public override void Enter()
    {
        base.Enter();
        eye.SetVelocityToZero();
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