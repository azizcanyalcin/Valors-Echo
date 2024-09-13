using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private RectTransform rectTransform;
    private Slider slider;
    private CharacterStats stats => GetComponentInParent<CharacterStats>();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        UpdateHealthUI();
    }

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        stats.onHealthChanged += UpdateHealthUI;
    }
    private void UpdateHealthUI()
    {
        slider.maxValue = stats.maxHealth.GetValue();
        slider.value = stats.currentHealth;
    }
    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (stats != null)
            stats.onHealthChanged -= UpdateHealthUI;
    }
    private void FlipUI() => rectTransform.Rotate(0, 180, 0);
}