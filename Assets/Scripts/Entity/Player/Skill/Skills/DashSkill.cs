using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private SkillTreeSlotUI dashUnlockButton;
    public bool dashUnlocked { get; set; }
    [Header("Clone on Dash")]
    [SerializeField] private SkillTreeSlotUI cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on Arrival")]
    [SerializeField] private SkillTreeSlotUI cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }
    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked) dashUnlocked = true;
    }
    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked) cloneOnDashUnlocked = true;
    }
    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked) cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnArrival();
        UnlockCloneOnDash();
    }
    
}