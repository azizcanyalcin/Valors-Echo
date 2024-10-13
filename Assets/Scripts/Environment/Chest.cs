using UnityEngine;
using UnityEngine.UI;

public class Chest : AnimatedObject
{
    [SerializeField] private ItemDrop itemDrop;
    protected override void Start()
    {
        base.Start();
        itemDrop = GetComponent<ItemDrop>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
    }
    
    protected override void OpenObject()
    {
        base.OpenObject();
        AudioManager.instance.PlayDelayedSFX(22,transform,false,0.1f);
        if (itemDrop)
            itemDrop.GenerateDrop();
    }
}
