using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    protected float stateTimer;
    private string animatorBoolName;
    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.animatorBoolName = animatorBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemy.rb;
        enemy.animator.SetBool(animatorBoolName, true);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        enemy.animator.SetBool(animatorBoolName, false);
        enemy.AssignLastAnimator(animatorBoolName);

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
