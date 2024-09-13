using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class ModifierEffect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;
    [SerializeField] private StatType statType;


    public override void ExecuteEffect(Transform transform)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStat(buffAmount, buffDuration, stats.GetStat(statType));
        Debug.Log($"{stats.GetStat(statType)} is modified!");

    }

    

}