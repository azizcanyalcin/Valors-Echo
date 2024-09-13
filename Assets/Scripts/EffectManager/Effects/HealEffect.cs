using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage;
    public override void ExecuteEffect(Transform transform)
    {
        PlayerStats player = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healAmount = Mathf.RoundToInt(player.maxHealth.GetValue() * healPercentage);

        player.IncreaseHealth(healAmount);
    }
}