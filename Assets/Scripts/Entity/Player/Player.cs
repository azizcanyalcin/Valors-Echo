using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool isPlayerActive;
    public bool isPlayerDeadOnce;
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    public float coyoteTime;
    [HideInInspector] public float coyoteTimer;
    public float jumpBufferTime;
    [HideInInspector] public float jumpBufferTimer;


    [Header("Dash")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.3f;
    private float defaultDashSpeed;
    public float dashDirection { get; private set; }


    [Header("Attack")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }

    public PlayerBlackHoleState blackHoleState { get; private set; }

    public PlayerDeadState deadState { get; private set; }
    #endregion

    #region Components
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFx fx { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");

        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");

        deadState = new PlayerDeadState(this, stateMachine, "Dead");

    }
    protected override void Start()
    {
        base.Start();
        fx = GetComponent<PlayerFx>();
        skill = SkillManager.instance;
        CloseInteractionKey();

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        if (!isPlayerActive) return;
        if (Time.timeScale == 0) return;

        base.Update();

        stateMachine.currentState.Update();

        DashInput();
        CrystalInput();
        UsePotion();

        CheckCoyoteTime();
        CheckJumpBuffer();

    }

    private static void UsePotion()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UsePotion();
    }

    private void CheckCoyoteTime()
    {
        if (IsGroundDetected())
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }
    private void CheckJumpBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Space)) jumpBufferTimer = jumpBufferTime;
        else jumpBufferTimer -= Time.deltaTime;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    private void DashInput()
    {
        if (IsWallDetected()) return;

        if (!skill.dash.dashUnlocked) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
                dashDirection = facingDirection;

            stateMachine.ChangeState(dashState);
        }
    }


    private void CrystalInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();
    }

    public bool isBusy { get; private set; }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public override void SlowEntity(float slowRate, float slowDuration)
    {
        slowRate = 1 - slowRate;

        moveSpeed *= slowRate;
        jumpForce *= slowRate;
        dashSpeed *= slowRate / 2;
        animator.speed *= slowRate;

        Invoke("ReturnDefaultSpeed", slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;

    }
    protected override void SetupZeroKnockbackPower()
    {
        knockBackPower = new Vector2(0, 0);
    }

    private void CloseInteractionKey()
    {
        GameObject interactionKeyImage = GameObject.FindWithTag("InteractionKey");

        if (interactionKeyImage != null)
        {
            interactionKeyImage.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No GameObject found with the tag 'InteractionKey'");
        }
    }

    public override void Die()
    {
        base.Die();
        isPlayerDeadOnce = true;
        SaveManager.instance.SaveGame();
        stateMachine.ChangeState(deadState);
    }
}
