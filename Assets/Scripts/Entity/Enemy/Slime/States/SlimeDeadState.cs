using UnityEngine;

public class SlimeDeadState : EnemyState
{
    Slime slime;

    public SlimeDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Slime slime) : base(enemy, stateMachine, animatorBoolName)
    {
        this.slime = slime;
    }
    public override void Enter()
    {
        base.Enter();

        slime.animator.SetBool(slime.lastAnimatorBoolName, true);
        slime.animator.speed = 0;
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