using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkSkill : Skill
{
    [Header("Dark Skill Settings")]
    [SerializeField] private SkillTreeSlotUI darkUnlockButton;
    public bool darkUnlocked { get; private set; }

    [Header("Dark Skill Attributes")]
    [SerializeField] private GameObject darkPrefab;
    [SerializeField] private float darkDuration;

    [Header("Dark Skill Moving")]
    [SerializeField] private SkillTreeSlotUI darkMoveUnlockButton;
    public bool darkMoveUnlocked { get; private set; }
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool moveToEnemy;

    [Header("Dark Skill Explosion")]
    [SerializeField] private SkillTreeSlotUI darkExplosionUnlockButton;
    public bool darkExplosionUnlocked { get; private set; }
    [SerializeField] private bool canExplode;

    [Header("Dark Skill Mirage")]
    [SerializeField] private SkillTreeSlotUI darkCloneUnlockButton;
    public bool darkCloneUnlocked { get; private set; }
    [SerializeField] private bool cloneReplaceDarkorb;

    [Header("Dark Skill Multishot")]
    [SerializeField] private SkillTreeSlotUI darkMultiShotUnlockButton;
    public bool darkMultiShotUnlocked { get; private set; }
    [SerializeField] private int amountOfOrbs;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private float useTimeWindow;
    private List<GameObject> orbsLeft = new();

    private GameObject currentDarkOrb;

    protected override void Start()
    {
        base.Start();
        darkUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDarkSkill);
        darkMoveUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDarkMoveSkill);
        darkExplosionUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDarkExplosionSkill);
        darkCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDarkCloneSkill);
        darkMultiShotUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDarkMultiShotSkill);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiOrb())
        {
            return;
        }

        if (currentDarkOrb == null)
        {
            CreateDarkOrb();
        }
        else
        {
            if(moveToEnemy) { return; }

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentDarkOrb.transform.position;
            currentDarkOrb.transform.position = playerPos;

            if(cloneReplaceDarkorb)
            {
                SkillManager.Instance.clone.CreateClone(currentDarkOrb.transform, new Vector3(0, 0.7f));
                Destroy(currentDarkOrb);
            }
            else
            {
                currentDarkOrb.GetComponent<DarkSkillController>()?.FinishDark();
            }
        }
    }

    public void CreateDarkOrb()
    {
        currentDarkOrb = Instantiate(darkPrefab, player.transform.position, Quaternion.identity);
        DarkSkillController darkOrb = currentDarkOrb.GetComponent<DarkSkillController>();

        darkOrb.SetupDarkOrb(darkDuration, canExplode, moveToEnemy, moveSpeed, FindClosestEnemy(currentDarkOrb.transform), player);
    }

    public void ChoseRandomTarget()
    {
        currentDarkOrb.GetComponent<DarkSkillController>().ChooseRandomEnemy();
    }

    private bool CanUseMultiOrb()
    {
        if(canUseMultiStack)
        {
            if (orbsLeft.Count <= 0)
            {
                RefreshOrbs();
            }

            if(orbsLeft.Count > 0)
            {
                if (orbsLeft.Count == amountOfOrbs)
                {
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }

                cooldown = 0;
                GameObject orbToSpawn = orbsLeft[^1];
                GameObject newOrb = Instantiate(orbToSpawn, player.transform.position, Quaternion.identity);

                orbsLeft.Remove(orbToSpawn);

                newOrb.GetComponent<DarkSkillController>().SetupDarkOrb(
                    darkDuration, canExplode, moveToEnemy, moveSpeed, FindClosestEnemy(newOrb.transform), player);

                if (orbsLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefreshOrbs();
                }

                return true;
            }
        }

        return false;

    }

    private void RefreshOrbs()
    {
        int amountToAdd = amountOfOrbs - orbsLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            orbsLeft.Add(darkPrefab);
        }
    }

    private void ResetAbility()
    {
        if(cooldownTimer > 0) { return; }

        cooldownTimer = multiStackCooldown;
        RefreshOrbs();
    }

    protected override void CheckUnlock()
    {
        UnlockDarkSkill();
        UnlockDarkMoveSkill();
        UnlockDarkExplosionSkill();
        UnlockDarkCloneSkill();
        UnlockDarkMultiShotSkill();
    }

    #region Unlock Skills
    private void UnlockDarkSkill()
    {
        if (darkUnlockButton.unlocked)
        {
            darkUnlocked = true;
        }
    }

    private void UnlockDarkMoveSkill()
    {
        if (darkMoveUnlockButton.unlocked)
        {
            darkMoveUnlocked = true;
            moveToEnemy = true;
        }
    }

    private void UnlockDarkExplosionSkill()
    {
        if (darkExplosionUnlockButton.unlocked)
        {
            darkExplosionUnlocked = true;
            canExplode = true;
        }
    }

    private void UnlockDarkCloneSkill()
    {
        if (darkCloneUnlockButton.unlocked)
        {
            darkCloneUnlocked = true;
            cloneReplaceDarkorb = true;
        }
    }

    private void UnlockDarkMultiShotSkill()
    {
        if (darkMultiShotUnlockButton.unlocked)
        {
            darkMultiShotUnlocked = true;
            canUseMultiStack = true;
        }
    }
    #endregion
}
