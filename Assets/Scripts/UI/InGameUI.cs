using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private TextMeshProUGUI currentCurrency;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image potionImage;

    private SkillManager skills;

    [Header("Souls")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;
    void Start()
    {
        if (player != null)
        {
            player.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
    }
    void Update()
    {
        UpdateCurrency();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked) SetCooldown(dashImage);
        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked) SetCooldown(parryImage);
        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked) SetCooldown(crystalImage);
        if (Input.GetKeyDown(KeyCode.R) && skills.blackHole.isBlackHoleUnlocked) SetCooldown(blackholeImage);
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Potion) != null) SetCooldown(potionImage);

        CheckCooldown(crystalImage, skills.crystal.cooldown);
        CheckCooldown(parryImage, skills.parry.cooldown);
        CheckCooldown(dashImage, skills.dash.cooldown);
        CheckCooldown(blackholeImage, skills.blackHole.cooldown);
        CheckCooldown(potionImage, Inventory.instance.potionCooldown);
    }

    private void UpdateCurrency()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrency();

        currentCurrency.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = player.maxHealth.GetValue();
        slider.value = player.currentHealth;
    }

    private void SetCooldown(Image image)
    {
        if (image.fillAmount <= 0) image.fillAmount = 1;
    }
    private void CheckCooldown(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }
}
