using UnityEngine;

public class RootController : MonoBehaviour
{
    [SerializeField] private string targetLayer = "Player";
   
    [SerializeField] private Rigidbody2D rb;
    private CharacterStats stats;
    

    public void SetupRoot(CharacterStats stats)
    {
        this.stats = stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            // Deal damage to the player
            stats.DealPhysicalDamage(collision.GetComponent<CharacterStats>(), 2f);
            OnRootHit();
        }
    }

    private void OnRootHit()
    {
        
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 3f);
    }
}