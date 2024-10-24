using UnityEngine;

public class BodTeleportState : EnemyState
{
    Bod bod;
    public BodTeleportState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Bod bod) : base(enemy, stateMachine, animatorBoolName)
    {
        this.bod = bod;
    }

    public override void Enter()
    {
        base.Enter();

        bod.stats.SetImmunability(true);
        AudioManager.instance.PlaySFX(56,bod.transform,false);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (bod.CanGrasp()) stateMachine.ChangeState(bod.graspState);
            else stateMachine.ChangeState(bod.battleState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        bod.stats.SetImmunability(false);
    }

}