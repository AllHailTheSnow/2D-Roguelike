using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry Skill Settings")]
    [SerializeField] private SkillTreeSlotUI parryUnlockButton;
    public bool parryUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlotUI restoreUnlockedButton;
    public bool restoreUnlocked { get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    [SerializeField] private SkillTreeSlotUI parryCloneUnlockedButton;
    public bool parryCloneUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if(restoreUnlocked)
        {
            // Restore health on parry by a percentage of max health
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealth() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();
        // Initialize the skill tree buttons abd set up their listeners
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockRestore);
        parryCloneUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryClone);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockRestore();
        UnlockParryClone();
    }

    private void UnlockParry()
    {
        // Unlock the parry skill 
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }

    private void UnlockRestore()
    {
        // Unlock the restore health on parry skill
        if (restoreUnlockedButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }

    private void UnlockParryClone()
    {
        // Unlock the parry clone skill
        if (parryCloneUnlockedButton.unlocked)
        {
            parryCloneUnlocked = true;
        }
    }

    public void MakeCloneOnParry(Transform _spawnLocation)
    {
        //Create a clone on parry at enemy location
        if (parryCloneUnlocked)
        {
            SkillManager.Instance.clone.CreateCloneOnDelay(_spawnLocation);
        }
    }
}
