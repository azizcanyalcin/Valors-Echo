using UnityEngine;

public class BodAttackState : EnemyState
{
    private Bod bod;

    public BodAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Bod bod) : base(enemy, stateMachine, animatorBoolName)
    {
        this.bod = bod;
    }

    public override void Enter()
    {
        base.Enter();

        bod.teleportChance += 5;
        AudioManager.instance.PlaySFX(1, bod.transform, false);
    }
    public override void Update()
    {
        base.Update();

        bod.SetVelocityToZero();

        if (triggerCalled)
        {
            if (bod.CanTeleport()) stateMachine.ChangeState(bod.teleportState);
            else stateMachine.ChangeState(bod.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        bod.lastTimeAttacked = Time.time;
    }
}