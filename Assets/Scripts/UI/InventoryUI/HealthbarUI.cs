using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarUI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private CharacterStats stats => GetComponentInParent<CharacterStats>();
    private RectTransform healthbar => GetComponentInParent<RectTransform>();
    private Slider slider;
    [SerializeField] private GameObject healthBarVisual;

    public GameObject shockImage;
    public GameObject chillImage;
    public GameObject igniteImage;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
 
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealth();
        slider.value = stats.currentHP;

        if(stats.currentHP == stats.GetMaxHealth())
        {
            healthBarVisual.SetActive(false);
        }
        else
        {
            healthBarVisual.SetActive(true);
        }

        if (stats.currentHP <= 0)
        {
            gameObject.SetActive(false);
        }

        //turn off entity ui for player
        if (stats.GetComponentInParent<PlayerController>() != null)
        {
            healthBarVisual.SetActive(false);
        }
    }

    private void FlippedUI()
    {
        healthbar.Rotate(0, 180, 0);
    }

    private void OnEnable()
    {
        entity.OnFlipped += FlippedUI;
        stats.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if(entity != null)
        {
            entity.OnFlipped -= FlippedUI;
        }
        if(stats != null)
        {
            stats.OnHealthChanged -= UpdateHealthUI;
        }
    }
}
