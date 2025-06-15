using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatToolTipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statDescription;

    private void Start()
    {

    }

    public void ShowStatTooltip(string text)
    {
        statDescription.text = text;

        gameObject.SetActive(true);
    }

    public void HideStatTooltip()
    {
        gameObject.SetActive(false);
    }
}
