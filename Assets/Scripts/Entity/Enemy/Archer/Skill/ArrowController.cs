using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private string targetLayer = "Player";
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;
    private CharacterStats stats;
    private Vector2 moveDirection;

    private int facingDirection;

    private void Update()
    {
        if (canMove)
        {
            // Continue moving in the same direction
            rb.velocity = facingDirection * speed * moveDirection;
        }
    }

    public void SetupArrow(float speed, CharacterStats stats, Vector2 targetPosition, int facingDirection)
    {
        this.speed = speed;
        this.stats = stats;
        this.facingDirection = facingDirection;

        // Calculate direction to the target position (player's last known position)
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            // Deal damage to the player
            stats.DealPhysicalDamage(collision.GetComponent<CharacterStats>(), 2);
            OnArrowHit(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Stop the arrow if it hits the ground
            OnArrowHit(collision);
        }
    }

    private void OnArrowHit(Collider2D collision)
    {
        // Stop particle effects and disable collider
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;

        // Stop arrow movement
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Stick the arrow to the object it hit
        transform.parent = collision.transform;

        // Destroy the arrow after 1 second
        Destroy(gameObject, 1f);
    }

    public void FlipArrow()
    {
        if (flipped) return;

        // Reverse the arrow's direction
        moveDirection *= -1;
        flipped = true;
        transform.Rotate(0, 180, 0);

        targetLayer = "Enemy"; // Change target layer to enemy if needed
    }
}
