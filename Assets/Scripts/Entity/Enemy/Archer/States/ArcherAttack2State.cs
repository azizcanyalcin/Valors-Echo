using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttack2State : EnemyState
{
    private Archer archer;

    public ArcherAttack2State(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();
        archer.attackDistance++;
        AudioManager.instance.PlaySFX(36, archer.transform, false);
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

        archer.lastTimeAttack2 = Time.time;
        archer.attackDistance--;
    }
}