using System;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Sword Skill Properties")]
    [SerializeField] private SkillTreeSlotUI throwSwordUnlockButton;
    public bool throwSwordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Bouncing Sword Properties")]
    [SerializeField] private SkillTreeSlotUI boomerangBladeUnlockButton;
    public bool boomerangBladeUnlocked { get; private set; }
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Sword Properties")]
    [SerializeField] private SkillTreeSlotUI piercingBladeUnlockButton;
    public bool piercingBladeUnlocked { get; private set; }
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Sword Properties")]
    [SerializeField] private SkillTreeSlotUI shurikenBladeUnlockButton;
    public bool shurikenBladeUnlocked { get; private set; }
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float maxTravelDistance = 7f;
    [SerializeField] private float spinGravity = 1f;
    [SerializeField] private float hitCooldown = .35f;

    [Header("Sword Passive Properties")]
    [SerializeField] private SkillTreeSlotUI timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private SkillTreeSlotUI vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();

        throwSwordUnlockButton.GetComponent<Button>().onClick.AddListener(ThrowSwordUnlock);
        boomerangBladeUnlockButton.GetComponent<Button>().onClick.AddListener(BoomerangBladeUnlock);
        piercingBladeUnlockButton.GetComponent<Button>().onClick.AddListener(PiercingBladeUnlock);
        shurikenBladeUnlockButton.GetComponent<Button>().onClick.AddListener(ShurikenBladeUnlock);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(TimeStopUnlock);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(VulnerableUnlock);
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController swordController = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
        {
            swordController.SetupBounce(true, bounceAmount, bounceSpeed);
        }
        else if (swordType == SwordType.Pierce)
        {
            swordController.SetupPierce(pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            swordController.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
        }

        swordController.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    protected override void CheckUnlock()
    {
        ThrowSwordUnlock();
        BoomerangBladeUnlock();
        PiercingBladeUnlock();
        ShurikenBladeUnlock();
        TimeStopUnlock();
        VulnerableUnlock();
    }

    #region Sword Unlocks
    private void ThrowSwordUnlock()
    {
        if(throwSwordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            throwSwordUnlocked = true;
        }
    }

    private void BoomerangBladeUnlock()
    {
        if (boomerangBladeUnlockButton.unlocked)
        {
            swordType = SwordType.Bounce;
            boomerangBladeUnlocked = true;
        }
    }

    private void PiercingBladeUnlock()
    {
        if (piercingBladeUnlockButton.unlocked)
        {
            swordType = SwordType.Pierce;
            piercingBladeUnlocked = true;
        }
    }

    private void ShurikenBladeUnlock()
    {
        if (shurikenBladeUnlockButton.unlocked)
        {
            swordType = SwordType.Spin;
            shurikenBladeUnlocked = true;
        }
    }

    private void TimeStopUnlock()
    {
        if (timeStopUnlockButton.unlocked)
        {
            timeStopUnlocked = true;
        }
    }

    private void VulnerableUnlock()
    {
        if (vulnerableUnlockButton.unlocked)
        {
            vulnerableUnlocked = true;
        }
    }
    #endregion

    private void SetupGravity()
    {
        if(swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if(swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }

    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDir = mousePos - playerPos;

        return aimDir;
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 pos = (Vector2)player.transform.position + new Vector2(AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return pos;
    }
    #endregion
}
