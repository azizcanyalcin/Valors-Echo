using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private Player player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    private float cloneTimer;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int duplicateChance;
    private int facingDirection = 1;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - (Time.deltaTime * transitionSpeed));
            if (spriteRenderer.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform closestEnemy, bool canDuplicateClone, int duplicateChance, Player player, float damageMultiplier)
    {
        if (canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 4));
        }
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;
        this.closestEnemy = closestEnemy;
        this.canDuplicateClone = canDuplicateClone;
        this.duplicateChance = duplicateChance;
        this.player = player;
        this.damageMultiplier = damageMultiplier;
        FlipToClosestEnemy();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDirection(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDamage(enemyStats, damageMultiplier);

                if (player.skill.clone.canAttack)
                {
                    Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    if (weaponData != null) weaponData.Effect(hit.transform);
                }

                if (canDuplicateClone && Random.Range(0, 100) < duplicateChance)
                {
                    SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(1f * facingDirection, 0));
                }
            }
        }
    }
    private void FlipToClosestEnemy()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}