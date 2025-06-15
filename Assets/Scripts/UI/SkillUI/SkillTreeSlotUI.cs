using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UIController uiController;
    private Image skillImage;

    [Header("Skill Tree Slot UI")]
    [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;
    [SerializeField] private Color lockedSkillColour;

    [Header("Skill Info")]
    public bool unlocked;
    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlotUI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkill());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColour;

        uiController = GetComponentInParent<UIController>();

        //Check if the skill is already unlocked
        if (unlocked)
        {
            //Set the skill image to white
            skillImage.color = Color.white;
        }
    }

    public void UnlockSkill()
    {
        if(unlocked)
        {
            Debug.Log("Skill is already unlocked");
            return;
        }

        if (!PlayerManager.Instance.HaveEnoughMoney(skillPrice))
        {
            return;
        }

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Skill is locked");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Skill is unlocked");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiController.skillToolTipUI.ShowToolTip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiController.skillToolTipUI.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        //Check if the skill is already in the dictionary
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            //If it is then set the unlocked value to the value in the dictionary
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        //Check if the skill is already in the dictionary
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            //If it is then remove it and add the new value
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            //If it is not then just add it
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}
