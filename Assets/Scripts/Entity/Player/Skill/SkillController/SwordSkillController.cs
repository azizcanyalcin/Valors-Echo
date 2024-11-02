using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 35;
    [SerializeField] private int armorReductionAmount;
    [SerializeField] private int vulnerableDuration;
    [Header("Generals")]
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D swordCollider;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;
    private float freezeDuration;

    [Header("Bounce")]
    private float bounceSpeed = 20;
    private bool isBouncing;
    private int bounceAmount;

    [Header("Pierce")]
    private float pierceAmount;

    [Header("Spin")]
    private float maxSpinDistance;
    private float spinDuration;
    private float spinTimer;
    private bool isStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;

    private List<Transform> enemyTarget;
    private int targetIndex;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        swordCollider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if (canRotate)
            RotateTowardsVelocity();

        if (isReturning)
            ReturnToPlayer();

        if (isBouncing && enemyTarget.Count > 0)
            Bounce();

        if (isSpinning)
        {
            Spin();
        }
    }
    private void SwordDamage(Enemy enemy)
    {
        player.stats.DealElementalDamage(enemy.GetComponent<CharacterStats>());
        if (player.skill.sword.timeStopUnlocked) FreezeEnemy(enemy);
        if (player.skill.sword.vulnerableUnlocked) enemy.stats.ApplyVulnerable(vulnerableDuration);

        Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
        if (equipedAmulet != null) equipedAmulet.Effect(enemy.transform);
    }

    private void FreezeEnemy(Enemy enemy)
    {
        enemy.SlowEntity(1, freezeDuration);
        enemy.fx.ApplyFrostEffect(freezeDuration);
        enemy.stats.isFrozen = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordDamage(enemy);
        }

        DetectEnemyForBounce(collision, 15);
        StickToCollision(collision);
    }
    private void RotateTowardsVelocity()
    {
        transform.right = rb.velocity;
    }
    private void ReturnToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
        {
            player.CatchSword();
        }
    }

    public void DetectEnemyForBounce(Collider2D collision, int radius)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void Bounce()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
        {
            SwordDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
            targetIndex = (targetIndex + 1) % enemyTarget.Count;
            bounceAmount--;
            AudioManager.instance.PlaySFX(59, player.transform, false);
            if (bounceAmount <= 0)
            {
                isBouncing = false;
                isReturning = true;
            }

        }
    }

    public void Spin()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > maxSpinDistance && !isStopped)
        {
            StopSpinning();
        }

        if (isStopped)
        {
            spinTimer -= Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 0.8f * Time.deltaTime);

            hitTimer -= Time.deltaTime;

            if (hitTimer < 0 && isSpinning)
            {
                hitTimer = hitCooldown;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        SwordDamage(hit.GetComponent<Enemy>());
                }
            }

            if (spinTimer < 0)
            {
                isReturning = true;
                isSpinning = false;
            }
        }
    }

    public void StopSpinning()
    {
        isStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        swordCollider.enabled = true;
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
        animator.SetBool("Rotation", true);
    }

    public void StickToCollision(Collider2D collision)
    {

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopSpinning();
            return;
        }

        canRotate = false;
        swordCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }

    private void DestroySword()
    {
        Destroy(gameObject);
    }
    public void InitializeSword(Vector2 direction, float gravityScale, Player player, float freezeDuration)
    {
        this.player = player;
        this.freezeDuration = freezeDuration;

        rb.velocity = direction;
        rb.gravityScale = gravityScale;

        if (pierceAmount <= 0)
            animator.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroySword", 7);

    }

    public void InitializeBounce(bool isBouncing, int bounceAmount, float bounceSpeed)
    {
        this.isBouncing = isBouncing;
        this.bounceAmount = bounceAmount;
        this.bounceSpeed = bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void InitializePierce(int pierceAmount)
    {
        this.pierceAmount = pierceAmount;
    }

    public void InitializeSpin(bool isSpinning, float maxSpinDistance, float spinDuration, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxSpinDistance = maxSpinDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }
}