using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundedState : EnemyState
{
    protected Transform player;
    protected Slime slime;
    public SlimeGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Slime slime) : base(enemy, stateMachine, animatorBoolName)
    {
        this.slime = slime;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();
        if (slime.IsPlayerDetected() || Vector2.Distance(player.transform.position, slime.transform.position) < slime.agroDistance)
            stateMachine.ChangeState(slime.battleState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}
