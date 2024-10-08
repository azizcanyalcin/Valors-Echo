using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAttackState : EnemyState
{
    private Eye eye;

    public EyeAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Eye eye) : base(enemy, stateMachine, animatorBoolName)
    {
        this.eye = eye;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(37, eye.transform, false);
    }
    public override void Update()
    {
        base.Update();

        eye.SetVelocityToZero();
        if (triggerCalled)
            stateMachine.ChangeState(eye.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        eye.lastTimeAttacked = Time.time;
    }
}