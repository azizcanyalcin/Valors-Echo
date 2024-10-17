using UnityEngine;

public class Druid : Enemy
{
    Player player;
    [Header("Druid")]
    [SerializeField] public GameObject rootPrefab;
    [SerializeField] public GameObject ashPrefab;

    [Header("Cooldowns")]
    public float healCooldown;
    public float fireAttackCooldown;
    public float ashThrowCooldown;
    public float walkingFireCooldown;
    public float rootAttackCooldown;
    [HideInInspector] public float lastTimeHealed;
    [HideInInspector] public float lastTimeFireAttacked;
    [HideInInspector] public float lastTimeAshThrowed;
    [HideInInspector] public float lastTimeWalkingFired;
    [HideInInspector] public float lastTimeRootAttacked;

    #region States
    public DruidIdleState idleState { get; private set; }
    public DruidMoveState moveState { get; private set; }
    public DruidBattleState battleState { get; private set; }
    public DruidFireAttackState fireAttackState { get; private set; }
    public DruidAshThrowState ashThrowState { get; private set; }
    public DruidWalkingFireState walkingFireState { get; private set; }
    public DruidRootAttackState rootAttackState { get; private set; }
    public DruidHealState healState { get; private set; }
    public DruidDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new DruidIdleState(this, stateMachine, "Idle", this);
        moveState = new DruidMoveState(this, stateMachine, "Move", this);
        battleState = new DruidBattleState(this, stateMachine, "Move", this);
        deadState = new DruidDeadState(this, stateMachine, "Dead", this);
        fireAttackState = new DruidFireAttackState(this, stateMachine, "FireAttack", this);
        ashThrowState = new DruidAshThrowState(this, stateMachine, "AshThrow", this);
        walkingFireState = new DruidWalkingFireState(this, stateMachine, "FireWalk", this);
        rootAttackState = new DruidRootAttackState(this, stateMachine, "RootAttack", this);
        healState = new DruidHealState(this, stateMachine, "Heal", this);
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
    public void DruidRootTrigger()
    {
        float groundY = player.GetGroundYPosition();

        GameObject root = Instantiate(rootPrefab, new Vector3(player.transform.position.x, groundY + 1), Quaternion.identity);
        root.GetComponent<RootController>().SetupRoot(stats);
        GameObject root2 = Instantiate(rootPrefab, new Vector3(player.transform.position.x + 2, groundY + 1), Quaternion.identity);
        root2.GetComponent<RootController>().SetupRoot(stats);
    }
    public void DruidAshTrigger()
    {
        GameObject ash = Instantiate(ashPrefab, new Vector3(player.transform.position.x, player.transform.position.y), Quaternion.identity);
        ash.GetComponent<AshController>().SetupAsh(stats);

        // Create the mirrored version of the ash
        GameObject mirroredAsh = Instantiate(ashPrefab, new Vector3(player.transform.position.x + .8f, player.transform.position.y), Quaternion.identity);

        // Mirror the second ash by flipping its scale along the X-axis
        Vector3 mirroredScale = mirroredAsh.transform.localScale;
        mirroredScale.x *= -1; // Flip the X scale
        mirroredAsh.transform.localScale = mirroredScale;

        mirroredAsh.GetComponent<AshController>().SetupAsh(stats);
    }
    public void DruidHealTrigger()
    {
        stats.currentHealth += stats.maxHealth.GetValue() / 5;
        stats.TakeDamage(1);
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}