using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{

    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Regular Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Clone Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockCloneCrystalButton;
    [SerializeField] private bool canCreateClone;

    [Header("Explosive Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockExplosiveCrystalButton;
    [SerializeField] private bool canExplode;
    [SerializeField] private float explosiveCooldown;


    [Header("Moving Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Crystal")]
    [SerializeField] private SkillTreeSlotUI unlockMultiCrystalButton;
    [SerializeField] private bool canUseMultiCrystal;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float multiStackUsageCooldown;
    [SerializeField] private List<GameObject> crystalList = new();

    protected override void Start()
    {
        base.Start();
        InitializeCrystalButtons();
        CheckUnlock();
    }


    public override void UseSkill()
    {
        base.UseSkill();

        if (HandleMultiCrystal())
            return;

        if (!currentCrystal)
        {
            CreateCrystal();
        }
        else if (!canMoveToEnemy)
        {
            SwapCrystalAndPlayer();
        }
    }
    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        var crystalScript = currentCrystal.GetComponent<CrystalSkillController>();
        crystalScript.InitializeCrystal(crystalDuration, moveSpeed, canMoveToEnemy, canExplode, FindClosestEnemy(currentCrystal.transform), player);
    }

    public void ChooseRandomEnemy()
    {
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }

    private void SwapCrystalAndPlayer()
    {
        (currentCrystal.transform.position, player.transform.position) = (player.transform.position, currentCrystal.transform.position);

        if (canCreateClone)
        {
            SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
            Destroy(currentCrystal);
        }
        else
            currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
    }

    private bool HandleMultiCrystal()
    {
        if (canUseMultiCrystal)
        {
            if (crystalList.Count > 0)
            {
                if (crystalList.Count == amountOfStacks)
                    Invoke("ResetMultiCrystal", multiStackUsageCooldown);

                cooldown = 0;
                SpawnCrystalFromList();
                return true;
            }
        }

        return false;
    }

    private void SpawnCrystalFromList()
    {
        var crystalToSpawn = crystalList[^1];
        var newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
        crystalList.RemoveAt(crystalList.Count - 1);

        var crystalScript = newCrystal.GetComponent<CrystalSkillController>();
        crystalScript.InitializeCrystal(crystalDuration, moveSpeed, canMoveToEnemy, canExplode, FindClosestEnemy(newCrystal.transform), player);

        if (crystalList.Count == 0)
        {
            cooldown = multiStackCooldown;
            RefillCrystalList();
        }
    }

    private void RefillCrystalList()
    {
        int amountToAdd = amountOfStacks - crystalList.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalList.Add(crystalPrefab);
        }
    }

    private void ResetMultiCrystal()
    {
        if (cooldownTimer <= 0)
        {
            cooldownTimer = multiStackCooldown;
            RefillCrystalList();
        }

    }

    private void InitializeCrystalButtons()
    {
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneCrystal);
        unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
    }
    #region Unlocking Crystal Skills
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked) crystalUnlocked = true;
    }
    private void UnlockCloneCrystal()
    {
        if (unlockCloneCrystalButton.unlocked) canCreateClone = true;
    }
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveCrystalButton.unlocked)
        {
            canExplode = true;
            cooldown = explosiveCooldown;
        }
    }
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked) canMoveToEnemy = true;
    }
    private void UnlockMultiCrystal()
    {
        if (unlockMultiCrystalButton.unlocked) canUseMultiCrystal = true;
    }
    #endregion
    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockMovingCrystal();
        UnlockExplosiveCrystal();
        UnlockCloneCrystal();
        UnlockMultiCrystal();

    }
}