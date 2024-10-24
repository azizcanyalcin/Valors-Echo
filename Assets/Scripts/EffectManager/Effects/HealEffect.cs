using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage;
    public override void ExecuteEffect(Transform transform)
    {
        Player player = PlayerManager.instance.player;
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        
        AudioManager.instance.PlaySFX(54, player.transform, false);

        int healAmount = Mathf.RoundToInt(playerStats.maxHealth.GetValue() * healPercentage);

        playerStats.IncreaseHealth(healAmount);
    }
}