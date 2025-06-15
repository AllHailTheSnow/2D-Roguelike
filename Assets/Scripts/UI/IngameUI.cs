using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    private SkillManager skills;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider specialSlider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image darkImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image swordThrowImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private float currencyAmount;
    [SerializeField] private float increaseRate = 100;

    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.Instance;
    }

    private void Update()
    {
        UpdateCurrencyUI();
        UpdateSpecialSlider();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
        {
            SetCooldownOf(dashImage);
        }

        if(Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }

        if(Input.GetKeyDown(KeyCode.F) && skills.dark.darkUnlocked)
        {
            SetCooldownOf(darkImage);
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.throwSwordUnlocked)
        {
            SetCooldownOf(swordThrowImage);
        }

        if(Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
        {
            SetCooldownOf(blackholeImage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.Instance.GetEquipment(EquipmentType.Flask) != null)
        {
            SetCooldownOf(flaskImage);
        }

        GetCooldown(dashImage, skills.dash.cooldown);
        GetCooldown(parryImage, skills.parry.cooldown);
        GetCooldown(darkImage, skills.dark.cooldown);
        GetCooldown(swordThrowImage, skills.sword.cooldown);
        GetCooldown(blackholeImage, skills.blackhole.cooldown);
        GetCooldown(flaskImage, Inventory.Instance.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealth();
        slider.value = playerStats.currentHP;
    }

    private void UpdateSpecialSlider()
    {
        specialSlider.maxValue = PlayerManager.Instance.player.specialMeterMax;
        specialSlider.value = PlayerManager.Instance.player.specialMeter;
    }

    private void UpdateCurrencyUI()
    {
        if(currencyAmount < PlayerManager.Instance.GetCurrency())
        {
            currencyAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            currencyAmount = PlayerManager.Instance.GetCurrency();
        }

        currencyText.text = ((int)currencyAmount).ToString();
    }

    private void SetCooldownOf(Image image)
    {
        if (image.fillAmount <= 0)
        {
            image.fillAmount = 1;
        }
    }

    private void GetCooldown(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
