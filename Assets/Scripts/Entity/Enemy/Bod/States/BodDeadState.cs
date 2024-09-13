using UnityEngine;

public class BodDeadState : EnemyState
{
    Bod bod;
    public BodDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Bod bod) : base(enemy, stateMachine, animatorBoolName)
    {
        this.bod = bod;
    }
    public override void Enter()
    {
        base.Enter();

        bod.animator.SetBool(bod.lastAnimatorBoolName, true);
        bod.animator.speed = 0;
        enemy.capsuleCollider.enabled = false;

        stateTimer = .1f;
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}