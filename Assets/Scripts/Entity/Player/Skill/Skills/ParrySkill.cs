using System;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private SkillTreeSlotUI parryUnlockButton;
    public bool parryUnlocked {get; private set;}

    [Header("Restore Health")]
    [SerializeField] private SkillTreeSlotUI restoreHPUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restorePercentage;
    private float restoreAmount = 1;
    public bool restoreHPUnlocked {get; private set;}

    [Header("Parry Mirage")]
    [SerializeField] private SkillTreeSlotUI parryMirageUnlockButton;
    public bool parryMirageUnlocked {get; private set;}

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreHPUnlocked)
        {
            restoreAmount = player.stats.maxHealth.GetValue() - player.stats.currentHealth;

            player.stats.IncreaseHealth(Mathf.RoundToInt(restorePercentage * restoreAmount));
        }
    }

    protected override void Start()
    {
        base.Start();
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreHPUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockRestoreHP);
        parryMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryMirage);
    }
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked) parryUnlocked = true;
    }
    private void UnlockRestoreHP()
    {
        if (restoreHPUnlockButton.unlocked) restoreHPUnlocked = true;
    }
    private void UnlockParryMirage()
    {
        if (parryMirageUnlockButton.unlocked) parryMirageUnlocked = true;
    }

    public void CreateMirageOnParry(Transform transform)
    {
        if (parryMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(transform);
        }
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryMirage();
        UnlockRestoreHP();
    }
}