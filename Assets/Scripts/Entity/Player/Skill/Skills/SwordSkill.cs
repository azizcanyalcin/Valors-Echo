using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Sword Skill")]
    [SerializeField] private SkillTreeSlotUI swordUnlockButton;
    public bool swordUnlocked;
    [SerializeField] private GameObject swordPrefab;

    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeDuration;

    [Header("Bounce")]
    [SerializeField] private SkillTreeSlotUI bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce")]
    [SerializeField] private SkillTreeSlotUI pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin")]
    [SerializeField] private SkillTreeSlotUI spinUnlockButton;
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxSpinDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Passive")]
    [SerializeField] private SkillTreeSlotUI timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlotUI vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }
    private Vector2 finalDirection;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        InitializeUnlockButtons();
        LayerMask.NameToLayer("Sword");

    }
    protected override void Update()
    {
        SetupGravity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDirection = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
        SwordSwap();

    }
    #region Unlock Area
    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked) timeStopUnlocked = true;
    }
    private void UnlockVulnerable()
    {
        if (vulnerableUnlockButton.unlocked) vulnerableUnlocked = true;
    }
    private void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }
    private void UnlockBounce()
    {
        if (bounceUnlockButton.unlocked)
        {
            swordType = SwordType.Bounce;
        }
    }
    private void UnlockPierce()
    {
        if (pierceUnlockButton.unlocked)
        {
            swordType = SwordType.Pierce;
        }
    }
    private void UnlockSpin()
    {
        if (spinUnlockButton.unlocked)
        {
            swordType = SwordType.Spin;
        }
    }
    private void SwordSwap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            List<SwordType> unlockedSwords = new();

            if (bounceUnlockButton.unlocked) unlockedSwords.Add(SwordType.Bounce);
            if (pierceUnlockButton.unlocked) unlockedSwords.Add(SwordType.Pierce);
            if (spinUnlockButton.unlocked) unlockedSwords.Add(SwordType.Spin);

            if (unlockedSwords.Count == 0) return;

            int currentSwordIndex = unlockedSwords.IndexOf(swordType);
            if (currentSwordIndex == -1) currentSwordIndex = 0;
            currentSwordIndex = (currentSwordIndex + 1) % unlockedSwords.Count;

            swordType = unlockedSwords[currentSwordIndex];
        }
    }

    private void InitializeUnlockButtons()
    {
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpin);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
    }
    #endregion

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        switch (swordType)
        {
            case SwordType.Regular:
                newSwordScript.InitializeSword(finalDirection, swordGravity, player, freezeDuration);
                break;
            case SwordType.Bounce:
                AudioManager.instance.PlaySFX(27, player.transform, false);
                newSwordScript.InitializeBounce(true, bounceAmount, bounceSpeed);
                break;
            case SwordType.Pierce:
                AudioManager.instance.PlaySFX(48, player.transform, false);
                newSwordScript.InitializePierce(pierceAmount);
                break;
            case SwordType.Spin:
                AudioManager.instance.PlaySFX(27, player.transform, false);
                newSwordScript.InitializeSpin(true, maxSpinDistance, spinDuration, hitCooldown);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(swordType), swordType, null);
        }


        newSwordScript.InitializeSword(finalDirection, swordGravity, player, freezeDuration);

        player.AssignNewSword(newSword);

        SetDotsActive(false);
    }
    private void SetupGravity()
    {
        swordGravity = swordType switch
        {
            SwordType.Bounce => bounceGravity,
            SwordType.Pierce => pierceGravity,
            SwordType.Spin => spinGravity,
            SwordType.Regular => swordGravity,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    public void SetDotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounce();
        UnlockPierce();
        UnlockSpin();
        UnlockTimeStop();
        UnlockVulnerable();
    }
}