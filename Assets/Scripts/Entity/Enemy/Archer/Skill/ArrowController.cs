using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private string targetLayer = "Player";
    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;
    private CharacterStats stats;

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }
    public void SetupArrow(float speed,CharacterStats stats)
    {
        xVelocity = speed;
        this.stats = stats;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            //collision.GetComponent<CharacterStats>()?.TakeDamage(damage);

            stats.DealPhysicalDamage(collision.GetComponent<CharacterStats>(), 1);

            OnArrowHit(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            OnArrowHit(collision);
    }

    private void OnArrowHit(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, 1f);
    }

    public void FlipArrow()
    {
        if (flipped) return;

        xVelocity *= -1;
        flipped = true;
        transform.Rotate(0, 180, 0);

        targetLayer = "Enemy";
    }
}