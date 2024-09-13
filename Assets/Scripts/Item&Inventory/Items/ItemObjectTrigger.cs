using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject ItemObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !collision.GetComponent<CharacterStats>().isDead)
        {
            Debug.Log($"Item picked Up!!!!!!");
            ItemObject.PickUpItem();
        }
    }
}