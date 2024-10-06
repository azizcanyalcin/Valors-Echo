using UnityEngine;

public class AssassUltiState : EnemyState
{
    Assassin assass;
    public AssassUltiState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }
    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(38, assass.transform, false);
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

        assass.lastTimeUltimate = Time.time;
    }
}