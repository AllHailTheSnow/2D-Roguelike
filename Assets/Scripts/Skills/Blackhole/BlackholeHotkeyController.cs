using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
    private KeyCode myHotkey;
    private TMP_Text hotkeyText;
    private SpriteRenderer hotkeySprite;
    private Transform enemy;
    private BlackholeSkillController blackhole;

    public void SetupHotkey(KeyCode _myHotkey, Transform _enemy, BlackholeSkillController _blackhole)
    {
        hotkeyText = GetComponentInChildren<TextMeshProUGUI>();
        hotkeySprite = GetComponent<SpriteRenderer>();

        enemy = _enemy;
        blackhole = _blackhole;
        myHotkey = _myHotkey;
        hotkeyText.text = _myHotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackhole.AddEnemyToList(enemy);

            hotkeyText.color = Color.clear;
            hotkeySprite.color = Color.clear;
        }
    }
}
