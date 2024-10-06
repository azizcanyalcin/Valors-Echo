using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy
{
    Player player;
    [SerializeField] GameObject tornado;
    [SerializeField] private float tornadoSpeed;
    [Header("Jump")]
    public Vector2 jump;
    public float jumpCooldown;
    [HideInInspector] public float lastTimeJumped;

    [Header("Attack Cooldowns")]
    public float ultimateAttackCooldown;
    [HideInInspector] public float lastTimeUltimate;

    [Header("Defensive Cooldowns")]
    public float defendCooldown;
    [HideInInspector] public float lastTimeDefend;

    #region States
    public AssassIdleState idleState { get; private set; }
    public AssassMoveState moveState { get; private set; }
    public AssassJumpState jumpState { get; private set; }
    public AssassBattleState battleState { get; private set; }
    public AssassAttackState attackState { get; private set; }
    public AssassUltiState ultimateAttackState { get; private set; }
    public AssassDeadState deadState { get; private set; }
    public AssassDefendState defendState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new AssassIdleState(this, stateMachine, "Idle", this);
        moveState = new AssassMoveState(this, stateMachine, "Move", this);
        jumpState = new AssassJumpState(this, stateMachine, "Jump", this);
        battleState = new AssassBattleState(this, stateMachine, "Move", this);
        attackState = new AssassAttackState(this, stateMachine, "Attack", this);
        ultimateAttackState = new AssassUltiState(this, stateMachine, "Ultimate", this);
        deadState = new AssassDeadState(this, stateMachine, "Dead", this);
        defendState = new AssassDefendState(this, stateMachine, "Defend", this);
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
    public override void AnimationSpecialAttackTrigger()
    {
        base.AnimationSpecialAttackTrigger();
        GameObject newTornado = Instantiate(tornado, attackCheck.position, Quaternion.identity);
        newTornado.GetComponent<TornadoController>().SetupTornado(tornadoSpeed * facingDirection, stats, player.transform.position, facingDirection);
    }
    public void TeleportToPlayerTrigger()
    {
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
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
    }
}

