using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    protected Archer archer;
    protected Transform player;
    public ArcherGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();
        if (archer.IsPlayerDetected() || Vector2.Distance(player.transform.position, archer.transform.position) < archer.agroDistance)
            stateMachine.ChangeState(archer.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}