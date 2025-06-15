using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [Header("Blackhole variables")]
    [SerializeField] private SkillTreeSlotUI blackholeUnlockButton;
    public bool blackholeUnlocked { get; private set; }
    [SerializeField] private BlackholeSkillController blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;

    [Header("Clone attack variables")]
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;

    private BlackholeSkillController currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        AudioManager.Instance.PlaySFX(11, null);
        BlackholeSkillController blackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackhole = blackhole;
        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(BlackholeUnlock);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if(!currentBlackhole) { return false; }

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        BlackholeUnlock();
    }

    private void BlackholeUnlock()
    {
        if(blackholeUnlockButton.unlocked && !blackholeUnlocked)
        {
            blackholeUnlocked = true;
        }
    }
}
