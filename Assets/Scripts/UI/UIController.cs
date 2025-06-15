using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour, ISaveManager
{
    [Header("Death Elements")]
    [SerializeField] private FadeUI fadeUI;
    [SerializeField] private GameObject deathText;
    [SerializeField] private GameObject restartButton;
    [Space]
    [Header("UI Elements")]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private TextMeshProUGUI currencyText;
    [Space]
    public ItemTooltipUI toolTip;
    public StatToolTipUI statToolTipUI;
    public SkillTooltipUI skillToolTipUI;
    public CraftWindowUI craftWindowUI;
    [Space]
    [SerializeField] private VolumeSliderUI[] volumeSettings;

    private void Awake()
    {
        //Opens skill tree ui to assign button events then immediately closes again
        SwitchTo(skillTreeUI);
    }

    private void Start()
    {
        SwitchTo(ingameUI);
        toolTip.gameObject.SetActive(false);
        statToolTipUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateCurrencyUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKey(optionsUI);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKey(characterUI);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchWithKey(skillTreeUI);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchWithKey(craftingUI);
        }
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<FadeUI>() != null;
            if(!fadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(menu != null)
        {
            menu.SetActive(true);
            AudioManager.Instance.PlaySFX(13, null);
        }

        if(GameManager.Instance != null)
        {
            if (menu == ingameUI)
            {
                GameManager.Instance.PauseGame(false);
            }
            else
            {
                GameManager.Instance.PauseGame(true);
            }
        }
    }

    public void SwitchWithKey(GameObject menu)
    {
        if(menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForIngameUI();
            return;
        }

        SwitchTo(menu);
    }

    private void CheckForIngameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<FadeUI>() == null)
            {
                return;
            }
        }

        SwitchTo(ingameUI);
    }

    private void UpdateCurrencyUI()
    {
        currencyText.text = PlayerManager.Instance.GetCurrency().ToString();
    }

    public void SwitchOnEndScreen()
    {
        fadeUI.FadeOut();
        StartCoroutine(EndScreenRoutine());
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartScene();
    }

    public void QuitGame()
    {
        SaveManager.Instance.SaveGame();
        Application.Quit();
    }

    public void FinishGame()
    {
        fadeUI.FadeOut();
    }

    private IEnumerator EndScreenRoutine()
    {
        yield return new WaitForSeconds(1f);
        deathText.SetActive(true);
        yield return new WaitForSeconds(1f);
        restartButton.SetActive(true);
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (VolumeSliderUI item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (VolumeSliderUI item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
