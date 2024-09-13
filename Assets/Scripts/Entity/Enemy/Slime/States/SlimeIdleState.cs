using UnityEngine;

public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Slime slime) : base(enemy, stateMachine, animatorBoolName, slime)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = slime.idleTime;
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(slime.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        

    }
}