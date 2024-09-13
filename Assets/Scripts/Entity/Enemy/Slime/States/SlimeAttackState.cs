using UnityEngine;

public class SlimeAttackState : EnemyState
{
    protected Slime slime;
    public SlimeAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Slime slime) : base(enemy, stateMachine, animatorBoolName)
    {
        this.slime = slime;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        slime.SetVelocityToZero();
        if (triggerCalled)
            stateMachine.ChangeState(slime.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        slime.lastTimeAttacked = Time.time;
    }
}