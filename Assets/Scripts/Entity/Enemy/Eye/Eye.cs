using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Eye : Enemy
{
    Player player;
    private Patrol patrol;
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    [Header("Eye")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float safeDistance;

    #region States
    public EyeMoveState moveState { get; private set; }
    public EyeBattleState battleState { get; private set; }
    public EyeAttackState attackState { get; private set; }
    public EyeDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        moveState = new EyeMoveState(this, stateMachine, "Move", this);
        battleState = new EyeBattleState(this, stateMachine, "Move", this);
        attackState = new EyeAttackState(this, stateMachine, "Attack", this);
        deadState = new EyeDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(moveState);

        patrol = GetComponent<Patrol>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        player = PlayerManager.instance.player;
    }
    protected override void Update()
    {
        base.Update();
        if (DistanceToPlayer() <= attackCheckRadius || stats.currentHealth < stats.maxHealth.GetValue())
        {
            patrol.enabled = false;
            destinationSetter.enabled = true;
            aiPath.endReachedDistance = safeDistance;
            aiPath.whenCloseToDestination = CloseToDestinationMode.Stop;
        }
        if (aiPath.desiredVelocity.x >= 0.01f && facingDirection != 1 || aiPath.desiredVelocity.x <= 0.01f && facingDirection != -1) Flip();
    }
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);

        Vector3 direction = (player.transform.position - newArrow.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

        if (player.transform.position.x > transform.position.x)
        {
            newArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            newArrow.transform.rotation = Quaternion.Euler(0, 0, -angle);
            newArrow.transform.localScale = new Vector3(-newArrow.transform.localScale.x, newArrow.transform.localScale.y, newArrow.transform.localScale.z);
        }
        newArrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * facingDirection, stats, player.transform.position, facingDirection);
    }
    public bool IsWallBehind()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDirection, wallCheckDistance, whatIsGround);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
        Destroy(gameObject, 0.3f);
    }
}

