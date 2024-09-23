using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    Player player;
    [Header("Archer")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject diagonalArrowPrefab;
    [SerializeField] private GameObject arrowShowerPrefab;
    [SerializeField] private GameObject beamExtensionPrefab;
    [SerializeField] private float arrowSpeed;
    [Header("Jump")]
    public Vector2 jump;
    public float jumpCooldown;
    public float safeDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("Attack Cooldowns")]
    public float attack2Cooldown;
    public float attack3Cooldown;
    public float ultimateAttackCooldown;
    [HideInInspector] public float lastTimeAttack2;
    [HideInInspector] public float lastTimeAttack3;
    [HideInInspector] public float lastTimeUltimate;

    [Header("Defensive Cooldowns")]
    public float defendCooldown;
    [HideInInspector] public float lastTimeDefend;

    [Header("Behind Collision")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherAttack2State attack2State { get; private set; }
    public ArcherAttack3State attack3State { get; private set; }
    public ArcherUltimateAttackState ultimateAttackState { get; private set; }
    public ArcherStunState stunState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    public ArcherDefendState defendState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        attack2State = new ArcherAttack2State(this, stateMachine, "Attack2", this);
        attack3State = new ArcherAttack3State(this, stateMachine, "Attack3", this);
        ultimateAttackState = new ArcherUltimateAttackState(this, stateMachine, "Ultimate", this);
        stunState = new ArcherStunState(this, stateMachine, "Stun", this);
        deadState = new ArcherDeadState(this, stateMachine, "Dead", this);
        defendState = new ArcherDefendState(this, stateMachine, "Defend", this);
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

    public override void AnimationThirdAttackTrigger()
    {
        GameObject newArrowShower = Instantiate(arrowShowerPrefab, new Vector3(player.transform.position.x, player.transform.position.y + 1.5f), Quaternion.identity);
        newArrowShower.GetComponent<ArrowShower>().SetupArrowShower(stats);
    }
    public void AnimationUltimateTrigger()
    {
        GameObject beamExtension = Instantiate(beamExtensionPrefab, new Vector3(transform.position.x + (20 * facingDirection), transform.position.y + 0.302f), Quaternion.identity);
        beamExtension.GetComponent<BeamExtension>().SetupBeam(stats);
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

