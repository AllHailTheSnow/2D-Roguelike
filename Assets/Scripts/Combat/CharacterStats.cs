using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    armour,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage,
    health,
    elementalChance
}

public class CharacterStats : MonoBehaviour
{
    [Header("Health Stats")]
    public Stats maxHP;
    public int currentHP;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    private bool isVulnerable;

    [Header("Main Stats")]
    public Stats strength; //Increase damage by 1 and crit power by 1%
    public Stats agility; //Increase evasion by 1% and crit chance by 1%
    public Stats intelligence; //Increase magic resistance by 3 per point
    public Stats vitality; //Increase health by 3

    [Header("Offence Stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critPower; //Default 150% of damage

    [Header("Defence Stats")]
    public Stats armour;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Elemental Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;
    public Stats elementalChance; //Increase the chance to apply elements by 1% per point
    [SerializeField] private float elementDuration = 4f;

    public bool isIgnited; //Deal damage over time
    public bool isChilled; //Reduce armour by 20%
    public bool isShocked; //Reduce accuracy by 20%

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ignitedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int ignitedDamage;

    [SerializeField] private GameObject shockStrike;
    private int shockDamage;

    private EntityFX entityFX;

    public System.Action OnHealthChanged;


    protected virtual void Start()
    {
        currentHP = GetMaxHealth();
        critPower.SetDefaultValue(150);
        entityFX = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }

    //Deal damage to the target based on the strength and damage
    public virtual void DoDamage(CharacterStats targetStats)
    {
        bool criticalHit = false;

        //Check if the target can avoid the attack
        if (CheckTargetCanAvoid(targetStats) || targetStats.currentHP <= 0)
        {
            return;
        }

        targetStats.GetComponent<Entity>().SetupKnockbackDirection(transform);

        //Get the total damage from strength and damage
        int totalDamage = damage.GetValue() + strength.GetValue();

        //Check if the attack can crit
        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
            criticalHit = true;
        }

        entityFX.CreateHitFX(targetStats.transform, criticalHit);


        totalDamage = CheckTargetArmour(targetStats, totalDamage);
        targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(targetStats);
    }

    public virtual void IncreaseStatBy(int modifier, float timeDuration, Stats statToModify)
    {
        StartCoroutine(IncreaseStatRoutine(modifier, timeDuration, statToModify));
    }

    #region Magic Damage and Elements
    //Deal magical damage to the target based on the elements
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        //Get the total magical damage from all elements
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        //Get the total magical damage from all elements and intelligence
        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        //If all elements are 0, return as fail safe
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptToApplyElement(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptToApplyElement(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        if (Random.Range(0, 100) < elementalChance.GetValue())
        {

            //Check which element is the highest and apply that element
            bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
            bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
            bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

            //If all elements are equal, apply a random element
            while (!canApplyChill && !canApplyIgnite && !canApplyShock)
            {
                if (Random.value < 0.5f && _fireDamage > 0)
                {
                    canApplyIgnite = true;
                    _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                    return;
                }

                if (Random.value < 0.5f && _iceDamage > 0)
                {
                    canApplyChill = true;
                    _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                    return;
                }

                if (Random.value < 0.5f && _lightningDamage > 0)
                {
                    canApplyShock = true;
                    _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
                    return;
                }
            }

            if (canApplyIgnite)
            {
                _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
            }

            if (canApplyShock)
            {
                _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.1f));
            }

            _targetStats.ApplyElements(canApplyIgnite, canApplyChill, canApplyShock);
        }

    }

    //Apply the elements to the target
    public void ApplyElements(bool _ignited, bool _chilled, bool _shocked)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignited && canApplyIgnite)
        {
            isIgnited = _ignited;
            ignitedTimer = elementDuration;
            entityFX.IgniteFX(elementDuration);
        }

        if (_chilled && canApplyChill)
        {
            isChilled = _chilled;
            chilledTimer = elementDuration;
            float slowPercent = 0.2f;
            GetComponent<Entity>().SlowEntityBy(slowPercent, elementDuration);
            entityFX.ChillFX(elementDuration);
        }

        if (_shocked && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shocked);
            }
            else
            {
                if (GetComponent<PlayerController>() != null)
                {
                    return;
                }

                HitNearestTargetShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shocked)
    {
        if (isShocked) { return; }

        isShocked = _shocked;
        shockedTimer = elementDuration;
        entityFX.LightningFX(elementDuration);
    }

    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthBy(ignitedDamage);
            if (currentHP <= 0 && !isDead)
            {
                Death();
            }

            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    private void HitNearestTargetShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject thunder = Instantiate(shockStrike, transform.position, Quaternion.identity);

            thunder.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage)
    {
        ignitedDamage = _damage;
    }

    public void SetupShockStrikeDamage(int _damage)
    {
        shockDamage = _damage;
    }
    #endregion

    #region Stat Modifiers

    //Check the target's magic resistance and intelligence to reduce the total magical damage
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    //Check the target's armour to reduce the total damage
    protected int CheckTargetArmour(CharacterStats targetStats, int totalDamage)
    {
        if (targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(targetStats.armour.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= targetStats.armour.GetValue();
        }

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    //Take damage from the target
    public virtual void TakeDamage(int _damage)
    {
        if(isInvincible)
        {
            return;
        }

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        entityFX.StartCoroutine("FlashFXRoutine");

        if (currentHP <= 0 && !isDead)
        {
            Death();
        }

    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHP += _amount;

        if (currentHP > GetMaxHealth())
        {
            currentHP = GetMaxHealth();
        }

        if (OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if(isVulnerable)
        {
            _damage = Mathf.RoundToInt(_damage * 1.1f);
        }

        currentHP -= _damage;

        if(_damage > 0)
        {
            entityFX.CreatePopupText(_damage.ToString());
        }

        if (OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }

    public virtual void OnEvasion()
    {

    }

    //Check if the target can avoid the attack based on their evasion
    protected bool CheckTargetCanAvoid(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.evasion.GetValue() + targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    //Check if the attack can crit based on the crit chance
    protected bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }

        return false;
    }

    //Calculate the crit damage based on the crit power
    protected int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealth()
    {
        return maxHP.GetValue() + vitality.GetValue() * 5;
    }

    // Returns the stat that the effect will modify based on the buffType
    public Stats GetStat(StatType _statType)
    {
        if (_statType == StatType.strength)
        {
            return strength;
        }
        else if (_statType == StatType.agility)
        {
            return agility;
        }
        else if (_statType == StatType.intelligence)
        {
            return intelligence;
        }
        else if (_statType == StatType.vitality)
        {
            return vitality;
        }
        else if (_statType == StatType.damage)
        {
            return damage;
        }
        else if (_statType == StatType.critChance)
        {
            return critChance;
        }
        else if (_statType == StatType.critPower)
        {
            return critPower;
        }
        else if (_statType == StatType.armour)
        {
            return armour;
        }
        else if (_statType == StatType.evasion)
        {
            return evasion;
        }
        else if (_statType == StatType.magicResistance)
        {
            return magicResistance;
        }
        else if (_statType == StatType.fireDamage)
        {
            return fireDamage;
        }
        else if (_statType == StatType.iceDamage)
        {
            return iceDamage;
        }
        else if (_statType == StatType.lightningDamage)
        {
            return lightningDamage;
        }
        else if (_statType == StatType.health)
        {
            return maxHP;
        }
        else if (_statType == StatType.elementalChance)
        {
            return elementalChance;
        }
        else
        {
            return null;
        }
    }
    #endregion

    public void MakeVulnerable(float _duration)
    {
        StartCoroutine(VulnerableRoutine(_duration));
    }

    public void KillEntity()
    {
        Death();
    }

    public void MakeInvincible(bool _invincible)
    {
        isInvincible = _invincible;
    }

    protected virtual void Death()
    {
        isDead = true;
    }

    private IEnumerator IncreaseStatRoutine(int modifier, float timeDuration, Stats statToModify)
    {
        statToModify.AddModifier(modifier);
        yield return new WaitForSeconds(timeDuration);
        statToModify.RemoveModifier(modifier);
    }

    private IEnumerator VulnerableRoutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }
}
