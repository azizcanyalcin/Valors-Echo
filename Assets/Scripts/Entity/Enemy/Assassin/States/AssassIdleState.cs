using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassIdleState : AssassGroundedState
{
    public AssassIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName, assass)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = assass.idleTime;
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(assass.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        

    }
}