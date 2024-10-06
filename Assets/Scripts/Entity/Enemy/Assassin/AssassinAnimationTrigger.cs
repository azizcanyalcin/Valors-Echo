using UnityEngine;

public class AssassinAnimationTrigger : EnemyAnimationTrigger
{
    Assassin assass => GetComponentInParent<Assassin>();
    private void TeleportToPlayerTrigger() => assass.TeleportToPlayerTrigger();
}