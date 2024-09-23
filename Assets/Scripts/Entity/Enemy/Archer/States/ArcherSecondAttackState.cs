using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSecondAttackState : EnemyState
{
    private Archer archer;

    public ArcherSecondAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName,Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        archer.SetVelocityToZero();
        if (triggerCalled)
            stateMachine.ChangeState(archer.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        archer.lastTimeAttacked = Time.time;
    }
}