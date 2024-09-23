using UnityEngine;

public class ArcherDefendState : EnemyState
{
    Archer archer;
    int defaultArmor;
    int defaultMagicResistance;
    public ArcherDefendState(Enemy enemy, EnemyStateMachine stateMachine, string animatorBoolName, Archer archer) : base(enemy, stateMachine, animatorBoolName)
    {
        this.archer = archer;
    }

    public override void Enter()
    {
        base.Enter();
        defaultArmor = archer.stats.armor.GetValue();
        defaultMagicResistance = archer.stats.magicResistance.GetValue();

        archer.stats.armor.SetValue(999);
        archer.stats.magicResistance.SetValue(999);

    }

    public override void Exit()
    {
        base.Exit();
        archer.stats.armor.SetValue(defaultArmor);
        archer.stats.magicResistance.SetValue(defaultMagicResistance);
        archer.lastTimeDefend = Time.time;

    }
}