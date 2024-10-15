using UnityEngine;

public class DruidIdleState : DruidGroundedState
{
    public DruidIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName, druid)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = druid.idleTime;
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(druid.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();


    }
}