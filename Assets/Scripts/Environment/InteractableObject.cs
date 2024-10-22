using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
{
    public bool isOpened = false;
    protected bool canKeyEnable = true;
    [SerializeField] protected Image keyE;
    protected virtual void Start()
    {
        keyE.enabled = false;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canKeyEnable)
                keyE.enabled = true;
        }
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            keyE.enabled = false;
        }
    }
    
}