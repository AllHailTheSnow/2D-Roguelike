using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [SerializeField] private SkillTreeSlotUI dodgeUnlockButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlotUI dodgeCloneUnlockButton;
    public bool dodgeCloneUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        dodgeUnlockButton.GetComponent<Button>().onClick.AddListener(DodgeUnlock);
        dodgeCloneUnlockButton.GetComponent<Button>().onClick.AddListener(DodgeCloneUnlock);
    }

    protected override void CheckUnlock()
    {
        DodgeUnlock();
        DodgeCloneUnlock();
    }

    private void DodgeUnlock()
    {
        if(dodgeUnlockButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.Instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void DodgeCloneUnlock()
    {
        if (dodgeCloneUnlockButton.unlocked)
        {
            dodgeCloneUnlocked = true;
        }
    }

    public void CreateCloneDodge()
    {
        if(dodgeCloneUnlocked)
        {
            if(Random.Range(0, 100) < 20)
            {
                SkillManager.Instance.clone.CreateClone(player.anim.transform, new Vector3(2 * player.facingDir, 0));
            }
        }
    }
}
