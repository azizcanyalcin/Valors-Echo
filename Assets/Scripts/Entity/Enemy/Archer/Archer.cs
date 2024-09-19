using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    Player player;
    [Header("Archer")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    public Vector2 jump;
    public float jumpCooldown;
    public float safeDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("Behind Collision")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunState stunState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        stunState = new ArcherStunState(this, stateMachine, "Stun", this);
        deadState = new ArcherDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        player = PlayerManager.instance.player;
    }
    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }

        return false;
    }
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);
        newArrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * facingDirection, stats, player.transform.position, facingDirection);
    }
    public bool IsGroundBehind()
    {
        return Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }
    public bool IsWallBehind()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDirection, wallCheckDistance, whatIsGround);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);
    }
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}

