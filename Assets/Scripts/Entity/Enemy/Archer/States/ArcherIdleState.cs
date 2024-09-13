using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherIdleState : ArcherGroundedState
{
   
    public ArcherIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName, archer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = archer.idleTime;
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(archer.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        

    }
}