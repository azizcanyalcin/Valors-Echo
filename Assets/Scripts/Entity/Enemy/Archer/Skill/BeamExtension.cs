using UnityEngine;

public class BeamExtension : MonoBehaviour
{
    [SerializeField] private string targetLayer = "Player";

    [SerializeField] private Rigidbody2D rb;
    private CharacterStats stats;


    public void SetupBeam(CharacterStats stats)
    {

        this.stats = stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            stats.DealPhysicalDamage(collision.GetComponent<CharacterStats>(), 10);
            OnArrowHit();
        }
    }

    private void OnArrowHit()
    {

        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 3f);
    }
}