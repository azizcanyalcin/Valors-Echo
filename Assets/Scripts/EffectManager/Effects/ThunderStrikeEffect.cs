using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform transform)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySFX(51,transform,true);
        Destroy(newThunderStrike, 1f);
    }
}