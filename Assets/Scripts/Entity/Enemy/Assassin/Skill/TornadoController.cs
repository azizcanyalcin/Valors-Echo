using UnityEngine;

public class TornadoController : MonoBehaviour
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

    public void SetupTornado(float speed, CharacterStats stats, Vector2 targetPosition, int facingDirection)
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
            stats.DealPhysicalDamage(collision.GetComponent<CharacterStats>(), 1);
        }
        OnTornadoHit();
    }

    private void OnTornadoHit()
    {
        // Destroy the arrow after 1 second
        Destroy(gameObject, 0.4f);
    }
}
