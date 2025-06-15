using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> hotkeyList;

    // Blackhole variables
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;
    private float blackholeTimer;

    // Clone attack variables
    public float cloneAttackTimer;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private bool cloneAttackReleased;
    private bool playerCanDisappear = true;

    // Hotkey variables
    private List<Transform> targets = new();
    private List<GameObject> createdHotkey = new();
    private bool canCreateHotkeys = true;

    public bool playerCanExitState { get; private set; }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        if(blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.Instance.clone.replaceDarkOrb)
        {
            playerCanDisappear = false;
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            float yOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
                yOffset = 0.7f;
            }
            else
            {
                xOffset = -2;
                yOffset = 0.7f;
            }

            if(SkillManager.Instance.clone.replaceDarkOrb)
            {
                SkillManager.Instance.dark.CreateDarkOrb();
                SkillManager.Instance.dark.ChoseRandomTarget();
            }
            else
            {
                SkillManager.Instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, yOffset));
            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke(nameof(FinishBlackholeAbility), 1f);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (SkillManager.Instance.clone.replaceDarkOrb)
        {
            cloneAttackReleased = true;
            canCreateHotkeys = false;
            return;
        }

        if(targets.Count <= 0)
        {
            return;
        }

        DestroyHotkeys();
        cloneAttackReleased = true;
        canCreateHotkeys = false;

        if(playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.Instance.player.entityFX.MakeTransparent(true);
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotkeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    private void CreateHotkey(Collider2D collision)
    {
        if (SkillManager.Instance.clone.replaceDarkOrb)
        {
            return;
        }

        if (hotkeyList.Count <= 0)
        {
            return;
        }

        if(!canCreateHotkeys) { return; }

        GameObject newHotKey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkey.Add(newHotKey);
        KeyCode chosenKey = hotkeyList[Random.Range(0, hotkeyList.Count)];
        hotkeyList.Remove(chosenKey);

        BlackholeHotkeyController newHotKeyScript = newHotKey.GetComponent<BlackholeHotkeyController>();

        newHotKeyScript.SetupHotkey(chosenKey, collision.transform, this);
    }

    private void DestroyHotkeys()
    {
        if(createdHotkey.Count <= 0)
        {
            return;
        }

        for(int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    public void AddEnemyToList(Transform enemyTransform)
    {
        targets.Add(enemyTransform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            collision.GetComponent<Enemy>().SetVelocityZero();
            CreateHotkey(collision);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }
}
