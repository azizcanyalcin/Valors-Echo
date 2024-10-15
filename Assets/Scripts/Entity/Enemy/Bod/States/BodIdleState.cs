using UnityEngine;

public class BodIdleState : EnemyState
{
    Bod bod;
    private Player player;
    public BodIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Bod bod) : base(enemy, stateMachine, animatorBoolName)
    {
        this.bod = bod;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = bod.idleTime;
        player = PlayerManager.instance.player;
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0 && bod.isTriggered) stateMachine.ChangeState(bod.battleState);
    }

    public override void Exit()
    {
        base.Exit();


    }
}