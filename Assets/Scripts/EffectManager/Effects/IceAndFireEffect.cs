using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 velocity;
    public override void ExecuteEffect(Transform transform)
    {
        Player player = PlayerManager.instance.player;

        bool isThirdAttack = player.primaryAttackState.comboCounter == 2;

        if (isThirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, transform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = velocity * player.facingDirection;
            Destroy(newIceAndFire, 2f);
        }
    }
}