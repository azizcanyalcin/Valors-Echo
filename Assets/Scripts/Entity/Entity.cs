using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }
    #endregion

    [Header("KnockBack")]
    [SerializeField] protected Vector2 knockBackPower;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;
    public int knockBackDirection { get; private set; }

    [Header("Collision")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;
    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        capsuleCollider = GetComponent<CapsuleCollider2D>();

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    protected virtual void Update()
    {

    }
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    public float GetGroundYPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, whatIsGround);

        if (hit.collider != null)
        {
            return hit.point.y;
        }


        return transform.position.y;
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        onFlipped?.Invoke();
    }
    public virtual void FlipController(float xVelocity)
    {
        if (xVelocity > 0 && !facingRight)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }
    public virtual void SetDefaultFacingDirection(int direction)
    {
        facingDirection = direction;
        if (facingDirection == -1) facingRight = false;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetVelocityToZero()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }
    public virtual void DamageImpact()
    {
        StartCoroutine("KnockBack");
    }

    protected virtual IEnumerator KnockBack()
    {
        isKnocked = true;

        if (knockBackPower.x > 0 || knockBackPower.y > 0)
            rb.velocity = new Vector2(knockBackPower.x * knockBackDirection, knockBackPower.y);

        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }
    public virtual void SetupKnockBackDirection(Transform damageDirection)
    {
        if (damageDirection.position.x > transform.position.x) knockBackDirection = -1;
        else if (damageDirection.position.x < transform.position.x) knockBackDirection = 1;
    }
    public void SetupKnockBackPower(Vector2 power) => knockBackPower = power;
    protected virtual void SetupZeroKnockbackPower()
    {

    }
    public virtual void SlowEntity(float slowRate, float slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        animator.speed = 1;
    }
    public virtual void Die()
    {
    }

}
