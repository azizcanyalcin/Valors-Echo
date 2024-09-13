using UnityEngine;

public class BodGraspState : EnemyState
{
    Bod bod;
    private int stack;
    private float cooldown;
    private float timer;
    public BodGraspState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Bod bod) : base(enemy, stateMachine, animatorBoolName)
    {
        this.bod = bod;
    }

    public override void Enter()
    {
        base.Enter();
        cooldown = bod.graspCooldown;
        stack = bod.graspStack;
        timer = cooldown + .5f;

    }
    public override void Update()
    {
        base.Update();


        timer -= Time.deltaTime;

        if (CanCast())
        {
            bod.CastGrasp();
            stack--;
            timer = cooldown;
        }
        if (stack <= 0) stateMachine.ChangeState(bod.teleportState);
    }
    private bool CanCast() => stack > 0 && timer < 0;

    public override void Exit()
    {
        base.Exit();
        bod.graspCastedTime = Time.time;
    }
}