using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassAttackState : EnemyState
{
    private Assassin assass;

    public AssassAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(37, assass.transform, false);
    }
    public override void Update()
    {
        base.Update();

        assass.SetVelocityToZero();
        if (triggerCalled)
            stateMachine.ChangeState(assass.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        assass.lastTimeAttacked = Time.time;
    }
}