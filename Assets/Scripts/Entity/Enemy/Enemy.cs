using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public string lastAnimatorBoolName { get; private set; }
    public bool isTriggered;
    [Header("Stun")]
    public float stunDuration;
    public Vector2 stunKnockBackPower;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Movement")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    float defaultMoveSpeed;

    [Header("Attack")]
    public float attackDistance;
    public float agroDistance = 2;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    protected override void Awake()
    {
        base.Awake();
        defaultMoveSpeed = moveSpeed;
        stateMachine = new EnemyStateMachine();
    }
    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFX>();
        cd = GetComponent<CapsuleCollider2D>();
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    public virtual void AssignLastAnimator(string animatorBoolName)
    {
        lastAnimatorBoolName = animatorBoolName;
    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialAttackTrigger() { }
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public virtual void FreezeTime(bool isFrozen)
    {

        if (isFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
            fx.ApplyFrostEffect(1);
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
            spriteRenderer.color = Color.white;
        }
    }
    public virtual void UseFreezeTimeCouroutine(float duration) => StartCoroutine(FreezeTimeCouroutine(duration));
    protected virtual IEnumerator FreezeTimeCouroutine(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }
    public override void SlowEntity(float slowRate, float slowDuration)
    {
        slowRate = 1 - slowRate;

        moveSpeed *= slowRate;
        animator.speed *= slowRate;

        Invoke("ReturnDefaultSpeed", slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }
}

