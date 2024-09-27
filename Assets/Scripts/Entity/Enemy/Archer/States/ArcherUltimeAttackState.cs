using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUltimateAttackState : EnemyState
{
    private Archer archer;

    public ArcherUltimateAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(38, archer.transform, false);
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

        archer.lastTimeUltimate = Time.time;
    }
}