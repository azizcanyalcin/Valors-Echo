using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotkeyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private KeyCode hotKey;
    private TextMeshProUGUI hotKeytext;
    private Transform enemy;
    private BlackHoleSkillController blackHoleSkillController;
    public void SetUpHotKey(KeyCode hotKey, Transform enemy, BlackHoleSkillController blackHoleSkillController)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hotKeytext = GetComponentInChildren<TextMeshProUGUI>();

        this.enemy = enemy;
        this.blackHoleSkillController = blackHoleSkillController;
        this.hotKey = hotKey;
        hotKeytext.text = hotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            blackHoleSkillController.AddEnemyToList(enemy);

            hotKeytext.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}