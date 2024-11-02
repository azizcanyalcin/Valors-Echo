using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UIManager uiManager;
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;


    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null) statNameText.text = statName;
    }
    private void Awake()
    {
        uiManager = GetComponentInParent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager component not found in parent hierarchy!");
        }
    }
    private void Start()
    {
        
        UpdateStatValueText();
    }

    public void UpdateStatValueText()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            statValueText.text = statType switch
            {
                StatType.strength => playerStats.strength.GetValue().ToString(),
                StatType.agility => playerStats.agility.GetValue().ToString(),
                StatType.intelligence => playerStats.intelligence.GetValue().ToString(),
                StatType.vitality => playerStats.vitality.GetValue().ToString(),
                StatType.damage => (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString(),
                StatType.critChance => (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString(),
                StatType.critPower => (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString(),
                StatType.maxHealth => (playerStats.maxHealth.GetValue() + playerStats.vitality.GetValue() * Random.Range(3, 5)).ToString(),
                StatType.armor => (playerStats.armor.GetValue() + playerStats.vitality.GetValue() * 3).ToString(),
                StatType.evasion => (playerStats.evasion.GetValue() + playerStats.agility.GetValue() * 3).ToString(),
                StatType.magicResistance => (playerStats.magicResistance.GetValue() + playerStats.agility.GetValue() * 3).ToString(),
                StatType.fireDamage => (playerStats.fireDamage.GetValue() + playerStats.intelligence.GetValue()).ToString(),
                StatType.iceDamage => (playerStats.iceDamage.GetValue() + playerStats.intelligence.GetValue()).ToString(),
                StatType.lightningDamage => (playerStats.lightningDamage.GetValue() + playerStats.intelligence.GetValue()).ToString(),
                _ => "Unknown Stat",
            };
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiManager.statToolTip.ShowStatTooltip(statDescription);
        AudioManager.instance.PlaySFX(46);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiManager.statToolTip.HideStatTooltip();
    }
}