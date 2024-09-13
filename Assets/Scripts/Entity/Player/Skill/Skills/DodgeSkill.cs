using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] SkillTreeSlotUI unlockDodgeButton;
    public bool dodgeUnlocked { get; private set; }
    [SerializeField] private int evasion;

    [Header("Mirage Dodge")]
    [SerializeField] SkillTreeSlotUI unlockMirageDodgeButton;
    public bool mirageDodgeUnlocked { get; private set; }


    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }
    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasion);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }
    private void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked) mirageDodgeUnlocked = true;
    }
    public void CreateMirageOnDodge()
    {
        if (mirageDodgeUnlocked) SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2*player.facingDirection, 0));

    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }
}