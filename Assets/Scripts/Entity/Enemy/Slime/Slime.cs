using System.Runtime;
using UnityEngine;

public enum SlimeType
{
    big,
    medium,
    small
}
public class Slime : Enemy
{
    [Header("Slime Creation")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int amountOfSlimes;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minVelocity;
    [SerializeField] private Vector2 maxVelocity;

    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunState stunState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        SetDefaultFacingDirection(-1);

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunState = new SlimeStunState(this, stateMachine, "Stun", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

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

    private void CreateSlimes(int amount, GameObject slimePrefab)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newSlime = Instantiate(slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Slime>().SetupSlime(facingDirection);
        }
    }
    public void SetupSlime(int facingDirection)
    {
        if (facingDirection != this.facingDirection) Flip();
        
        float xVelocity = Random.Range(minVelocity.x, maxVelocity.x);
        float yVelocity = Random.Range(minVelocity.y, maxVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDirection, yVelocity);
        Invoke("CancelKnockBack", 1.5f);
    }
    private void CancelKnockBack() => isKnocked = false;
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);

        if (slimeType == SlimeType.small) return;

        CreateSlimes(amountOfSlimes, slimePrefab);
        Destroy(this, 2f);
    }

}