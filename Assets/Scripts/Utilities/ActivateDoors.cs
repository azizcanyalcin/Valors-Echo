using System.Linq;
using UnityEngine;

public class ActivateDoors : MonoBehaviour
{
    [SerializeField] private GameObject[] agent;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (agent.Where(a => a != null).Count() == 0)
            boxCollider.isTrigger = true;
        else
            boxCollider.isTrigger = false;
    }
}
