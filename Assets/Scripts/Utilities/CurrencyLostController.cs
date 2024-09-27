using UnityEngine;

public class CurrencyLostController : MonoBehaviour
{
    public int currency;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.currency += currency;
            Destroy(gameObject);
        }
    }
}