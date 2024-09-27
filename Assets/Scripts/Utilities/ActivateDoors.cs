using UnityEngine;

public class ActivateDoors : MonoBehaviour
{
    [SerializeField] private GameObject agent;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (!agent) boxCollider.isTrigger = true;
        else boxCollider.isTrigger = false;
    }
}