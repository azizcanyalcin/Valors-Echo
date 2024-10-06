using UnityEngine;

public class AssassJumpState : EnemyState
{
    Player player;
    private Assassin assass;
    public AssassJumpState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Assassin assass) : base(enemy, stateMachine, animatorBoolName)
    {
        this.assass = assass;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player;
        AudioManager.instance.PlayDelayedSFX(37, assass.transform, false, 0.3f);
        JumpToPlayer();
    }
    private void JumpToPlayer()
    {
        rb.velocity = new Vector2(assass.jump.x * assass.facingDirection, assass.jump.y);
    }
    public override void Update()
    {
        base.Update();

        assass.animator.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0 && assass.IsGroundDetected())
        {
            assass.SetVelocityToZero();
            stateMachine.ChangeState(assass.battleState);
        }
    }
}