using UnityEngine;

public class GraspController : MonoBehaviour
{
    [SerializeField] Transform check;
    [SerializeField] Vector2 boxSize;
    [SerializeField] LayerMask playerLayer;
    private CharacterStats stats;

    public void SetupGrasp(CharacterStats stats) => this.stats = stats;
    
    private void AnimationTrigger()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(check.position, boxSize, playerLayer);
        foreach (var target in targets)
        {
            if (target.GetComponent<Player>())
            {
                target.GetComponent<Entity>().SetupKnockBackDirection(transform);
                stats.DealPhysicalDamage(target.GetComponent<CharacterStats>(), 1);
            }
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);

    private void SelfDestroy() => Destroy(gameObject);
}