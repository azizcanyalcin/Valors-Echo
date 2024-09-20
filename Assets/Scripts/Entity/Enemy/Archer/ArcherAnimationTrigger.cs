using UnityEngine;

public class ArcherAnimationTrigger : EnemyAnimationTrigger
{
    Archer archer => GetComponentInParent<Archer>();
    private void ThirdAttackTrigger() => archer.AnimationThirdAttackTrigger();
    private void UltimeAttackTrigger() => archer.AnimationUltimateTrigger();
}