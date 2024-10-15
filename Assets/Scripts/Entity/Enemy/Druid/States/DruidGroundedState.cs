using UnityEngine;

public class DruidGroundedState : EnemyState
{
    protected Druid druid;
    protected Transform player;
    public DruidGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Druid druid) : base(enemy, stateMachine, animatorBoolName)
    {
        this.druid = druid;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();
        if (druid.IsPlayerDetected() || Vector2.Distance(player.transform.position, druid.transform.position) < druid.agroDistance)
            stateMachine.ChangeState(druid.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}