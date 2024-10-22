using UnityEngine;

public class DruidAnimationTrigger : EnemyAnimationTrigger
{
    Druid druid => GetComponentInParent<Druid>();
    private void ThirdAttackTrigger() => druid.AnimationThirdAttackTrigger();
    private void RootTrigger() => druid.DruidRootTrigger();
    private void AshTrigger() => druid.DruidAshTrigger();
    private void HealTrigger() => druid.DruidHealTrigger();
    private void FindPlayer() => druid.DruidPlayerPositionTrigger();
}