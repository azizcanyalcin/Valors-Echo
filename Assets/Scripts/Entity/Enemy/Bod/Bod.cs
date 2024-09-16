using Unity.VisualScripting;
using UnityEngine;

public class Bod : Enemy
{
    [Header("Teleport")]
    [SerializeField] BoxCollider2D arena;
    [SerializeField] Vector2 objectCheckSize;
    [HideInInspector] public float teleportChance;
    public float defaultTeleportChance = 25;


    [Header("Grasp Skill")]
    [SerializeField] private GameObject graspPrefab;
    [SerializeField] private float graspStateCooldown;
    public float graspCastedTime;
    public int graspStack = 3;
    public float graspCooldown;


    #region States
    public BodIdleState idleState { get; private set; }
    public BodBattleState battleState { get; private set; }
    public BodAttackState attackState { get; private set; }
    public BodDeadState deadState { get; private set; }
    public BodTeleportState teleportState { get; private set; }
    public BodGraspState graspState { get; private set; }
    //public BodMoveState moveState { get; private set; }
    //public BodStunState stunState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        SetDefaultFacingDirection(-1);

        idleState = new BodIdleState(this, stateMachine, "Idle", this);
        teleportState = new BodTeleportState(this, stateMachine, "Teleport", this);

        battleState = new BodBattleState(this, stateMachine, "Move", this);
        attackState = new BodAttackState(this, stateMachine, "Attack", this);
        graspState = new BodGraspState(this, stateMachine, "Grasp", this);

        deadState = new BodDeadState(this, stateMachine, "Idle", this);

        //stunState = new BodStunState(this, stateMachine, "Stun", this);
        //moveState = new BodMoveState(this, stateMachine, "Move", this);
    }

    protected override void Start()
    {
        base.Start();

        
        stateMachine.Initialize(attackState);

    }
    protected override void Update()
    {
        base.Update();
    }
    public void CastGrasp()
    {
        GameObject newGrasp = Instantiate(graspPrefab, GraspPosition(), Quaternion.identity);
        newGrasp.GetComponent<GraspController>().SetupGrasp(stats);
    }
    private static Vector3 GraspPosition()
    {
        Player player = PlayerManager.instance.player;
        Vector3 graspPosition;

        if (player.stateMachine.currentState == player.moveState)
        {
            graspPosition = new(player.transform.position.x + player.facingDirection * 3,
                player.transform.position.y + 1);
        }
        else
        {
            graspPosition = new(player.transform.position.x, player.transform.position.y + 1.5f);
        }

        return graspPosition;
    }

    public bool CanGrasp() => Time.time >= graspCastedTime + graspStateCooldown;
    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= teleportChance)
        {
            teleportChance = defaultTeleportChance;
            return true;
        }
        return false;
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            //stateMachine.ChangeState(stunState);
            return true;
        }

        return false;
    }
    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        var groundBelow = GroundBelow();
        if (groundBelow)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y); // - groundBelow.distance + (cd.size.y / 2)
        }

        if (!groundBelow || IsOverlappingObjects())
        {
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, whatIsGround);
    private bool IsOverlappingObjects() => Physics2D.BoxCast(transform.position, objectCheckSize, 0, Vector2.zero, 0, whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, objectCheckSize);
    }
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}