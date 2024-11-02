using UnityEngine;

public class BodAnimationTriggers : EnemyAnimationTrigger
{
    Bod bod => GetComponentInParent<Bod>();

    private void Relocate() => bod.Teleport();
    private void MakeInvisible() => bod.fx.MakeTransparent(true);
    private void MakeVisible() => bod.fx.MakeTransparent(false);
    
}