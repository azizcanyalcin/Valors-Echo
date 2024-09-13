using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class CrystalSkillController : MonoBehaviour
{
    private Player player;
    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D crystalCollider => GetComponent<CircleCollider2D>();
    private Transform closestEnemy;
    [SerializeField] private LayerMask whatIsEnemy;

    private float crystalDuration;

    private bool canMoveToEnemy;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private bool canExplode;


    public void InitializeCrystal(float crystalDuration, float moveSpeed, bool canMoveToEnemy, bool canExplode, Transform closestEnemy, Player player)
    {
        this.crystalDuration = crystalDuration;
        this.moveSpeed = moveSpeed;
        this.canMoveToEnemy = canMoveToEnemy;
        this.canExplode = canExplode;
        this.closestEnemy = closestEnemy;
        this.player = player;
    }

    private void Update()
    {
        crystalDuration -= Time.deltaTime;

        if (crystalDuration < 0)
            FinishCrystal();

        if (canGrow)
            Grow();

        if (canMoveToEnemy)
            Move();
    }

    public void FinishCrystal()
    {
        if (canExplode)
            ExplodeCrystal();
        else
            DestroyCrystal();
    }

    private void ExplodeCrystal()
    {

        canGrow = true;
        animator.SetTrigger("Explode");
    }

    public void DestroyCrystal()
    {
        Destroy(gameObject);
    }

    private void Grow()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    private void Move()
    {
        if (closestEnemy == null) return;
        transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, closestEnemy.position) < 1.5)
            FinishCrystal();
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders is not null)
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;

    }

    private void ExplodeCrystalTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, crystalCollider.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockBackDirection(transform);
                player.stats.DealElementalDamage(hit.GetComponent<CharacterStats>());
                Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if (equipedAmulet != null) equipedAmulet.Effect(hit.transform);
            }
        }
    }
}
