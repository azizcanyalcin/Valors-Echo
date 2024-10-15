using UnityEngine;

public class AshController : MonoBehaviour
{
    [SerializeField] private string targetLayer = "Player";

    [SerializeField] private Rigidbody2D rb;
    private CharacterStats stats;


    public void SetupAsh(CharacterStats stats)
    {

        this.stats = stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            stats.DealPhysicalDamage(collision.GetComponent<CharacterStats>(), 1);
            OnAshHit();
        }
    }

    private void OnAshHit()
    {

        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 3f);
    }
}