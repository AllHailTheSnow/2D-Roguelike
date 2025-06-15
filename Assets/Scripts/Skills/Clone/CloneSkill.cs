using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone Details")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float attackMultiplier;

    [Header("Attack Details")]
    [SerializeField] private SkillTreeSlotUI cloneAttackUnlockButton;
    public bool cloneAttackUnlocked { get; private set; }
    [SerializeField] private bool canAttack;
    [SerializeField] private float cloneAttackMultiplier;

    [Header("Attack Element Details")]
    [SerializeField] private SkillTreeSlotUI cloneAttackElementUnlockButton;
    public bool cloneAttackElementUnlocked { get; private set; }
    [SerializeField] private float attackElementMultiplier;

    [Header("Clone Duplication")]
    [SerializeField] private SkillTreeSlotUI cloneDuplicationUnlockButton;
    public bool cloneDuplicationUnlocked { get; private set; }
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Dark Orb")]
    [SerializeField] private SkillTreeSlotUI darkOrbUnlockButton;
    public bool darkOrbUnlocked { get; private set; }
    public bool replaceDarkOrb;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        cloneAttackElementUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttackElement);
        cloneDuplicationUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneDuplication);
        darkOrbUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDarkOrb);
    }

    public void CreateClone(Transform clonePos, Vector3 offset)
    {
        if(replaceDarkOrb)
        {
            SkillManager.Instance.dark.CreateDarkOrb();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(
            clonePos, cloneDuration, canAttack, offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }

    public void CreateCloneOnDelay(Transform enemyTransform)
    {
        StartCoroutine(CreateCloneDelayRoutine(enemyTransform, new Vector3(2 * player.facingDir, 0.7f)));
    }

    private IEnumerator CreateCloneDelayRoutine(Transform enemyTransform, Vector3 offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(enemyTransform, offset);
    }

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockCloneAttackElement();
        UnlockCloneDuplication();
        UnlockDarkOrb();
    }

    #region Unlock Skills
    private void UnlockCloneAttack()
    {
        if(cloneAttackUnlockButton.unlocked)
        {
            cloneAttackUnlocked = true;
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockCloneAttackElement()
    {
        if (cloneAttackElementUnlockButton.unlocked)
        {
            cloneAttackElementUnlocked = true;
            attackMultiplier = attackElementMultiplier;
        }
    }

    private void UnlockCloneDuplication()
    {
        if (cloneDuplicationUnlockButton.unlocked)
        {
            cloneDuplicationUnlocked = true;
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockDarkOrb()
    {
        if (darkOrbUnlockButton.unlocked)
        {
            darkOrbUnlocked = true;
            replaceDarkOrb = true;
        }
    }
    #endregion
}
