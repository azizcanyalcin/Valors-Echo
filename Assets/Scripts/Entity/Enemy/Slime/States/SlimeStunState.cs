using UnityEngine;

public class SlimeStunState : EnemyState
{
    Slime slime;
    public SlimeStunState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Slime slime) : base(enemy, stateMachine, animatorBoolName)
    {
        this.slime = slime;
    }

    public override void Enter()
    {
        base.Enter();

        slime.fx.InvokeRepeating("RedBlink", 0, .1f);

        stateTimer = slime.stunDuration;
        rb.velocity = new Vector2(-1 * slime.facingDirection * slime.stunKnockBackPower.x, slime.stunKnockBackPower.y);
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(slime.idleState);
        if (rb.velocity.y < .1f && slime.IsGroundDetected())
        {
            slime.animator.SetTrigger("StunFold");
            slime.stats.SetImmunability(true);
        }
    }

    public override void Exit()
    {
        base.Exit();
        slime.fx.Invoke("CancelColorChange", 0);
        slime.stats.SetImmunability(false);
    }
}