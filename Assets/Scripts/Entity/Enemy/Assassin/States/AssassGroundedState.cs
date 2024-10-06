using UnityEngine;

public class AssassGroundedState : EnemyState
{
    protected Assassin assass;
    protected Transform player;

    public AssassGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();
        if (assass.IsPlayerDetected() || Vector2.Distance(player.transform.position, assass.transform.position) < assass.agroDistance)
            stateMachine.ChangeState(assass.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}