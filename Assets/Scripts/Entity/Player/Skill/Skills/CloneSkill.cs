using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float damageMultiplier;

    [Space]
    [Header("Clone Creation")]
    [SerializeField] private SkillTreeSlotUI cloneAttackUnlockButton;
    [SerializeField] private float cloneDamageMultiplier = 1;
    [SerializeField] public bool canAttack;

    [Header("Aggresive Clone")]
    [SerializeField] private SkillTreeSlotUI aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneDamageMultiplier = 1;
    public bool aggresiveCloneUnlocked { get; private set; }

    [Space]
    [Header("Multiple Clone")]
    [SerializeField] private SkillTreeSlotUI multipleCloneUnlockButton;
    [SerializeField] private float multipleCloneDamageMultiplier = 1;
    [SerializeField] private bool canDuplicateClone;
    [Range(0, 100)]
    [SerializeField] private int duplicateChance;

    [Space]
    [Header("Swap crystal and clone")]
    [SerializeField] private SkillTreeSlotUI swapCrystalWithCloneUnlockButton;
    [SerializeField] private bool disableCrystalWithBlackHole;
    public bool canSwapCrystalWithClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        swapCrystalWithCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwapCrystalWithClone);
    }

    protected override void Update()
    {
        base.Update();
        if (disableCrystalWithBlackHole)
            canSwapCrystalWithClone = player.stateMachine.currentState != player.blackHoleState;


    }
    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (canSwapCrystalWithClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>()
        .SetupClone(clonePosition, cloneDuration, canAttack, offset, canDuplicateClone, duplicateChance, player, damageMultiplier);
    }
    public void CreateCloneWithDelay(Transform enemy)
    {
        StartCoroutine(CreateCloneWithDelayCoroutine(enemy, new Vector3(2 * player.facingDirection, 0)));

    }

    private IEnumerator CreateCloneWithDelayCoroutine(Transform enemy, Vector3 offset)
    {
        canAttack = true;
        yield return new WaitForSeconds(.4f);
        CreateClone(enemy, offset);
        yield return new WaitForSeconds(.2f);
        canAttack = false;

    }

    #region Unlock Region

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            damageMultiplier = cloneDamageMultiplier;
            canAttack = true;
        }
    }

    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            damageMultiplier = aggresiveCloneDamageMultiplier;
            aggresiveCloneUnlocked = true;
        }
    }

    private void UnlockMultipleClone()
    {
        if (multipleCloneUnlockButton.unlocked)
        {
            damageMultiplier = multipleCloneDamageMultiplier;
            canDuplicateClone = true;
        }
    }
    private void UnlockSwapCrystalWithClone()
    {
        if (swapCrystalWithCloneUnlockButton.unlocked) canSwapCrystalWithClone = true;
    }

    #endregion

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggresiveClone();
        UnlockMultipleClone();
        UnlockSwapCrystalWithClone();
    }
}
