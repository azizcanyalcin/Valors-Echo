using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool isCloneCreated;
    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animatorBoolName) : base(player, stateMachine, animatorBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isCloneCreated = false;
        stateTimer = player.counterAttackDuration;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }
    public override void Update()
    {
        base.Update();

        player.SetVelocityToZero();

        CheckCounterAttack();

        ChangeStateToIdle();

    }

    private void CheckCounterAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<ArrowController>())
            {
                hit.GetComponent<ArrowController>().FlipArrow();
                SuccesfullCounterAttack();
            }

            if (hit.GetComponent<Enemy>() != null && hit.GetComponent<Enemy>().CanBeStunned())
            {
                SuccesfullCounterAttack();

                player.skill.parry.UseSkill(); // restore health on parry
                if (!isCloneCreated)
                {
                    isCloneCreated = true;
                    player.skill.parry.CreateMirageOnParry(hit.transform);
                }
            }
        }
    }

    private void SuccesfullCounterAttack()
    {
        stateTimer = 10;
        player.animator.SetBool("SuccessfulCounterAttack", true);
    }

    private void ChangeStateToIdle()
    {
        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}