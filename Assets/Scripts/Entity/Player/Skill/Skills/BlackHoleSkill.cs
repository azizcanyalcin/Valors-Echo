using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{
    [SerializeField] private SkillTreeSlotUI unlockBlackHoleButton;
    public bool isBlackHoleUnlocked;
    [SerializeField] private int amounOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;

    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    BlackHoleSkillController currentBlackHole;

    protected override void Start()
    {
        base.Start();
        unlockBlackHoleButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }
    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();

        currentBlackHole.SetupBlackHoleSkill(maxSize, growSpeed, shrinkSpeed, amounOfAttacks, cloneAttackCooldown, blackholeDuration);
    }

    public bool IsBlackHoleFinished()
    {
        if (currentBlackHole && currentBlackHole.canPlayerExitState)
        {
            currentBlackHole = null;
            return true;
        }
        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }

    private void UnlockBlackHole()
    {
        if (unlockBlackHoleButton.unlocked) isBlackHoleUnlocked = true;
    }
    protected override void CheckUnlock()
    {
        UnlockBlackHole();
    }
}