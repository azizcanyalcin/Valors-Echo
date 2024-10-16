using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
    }
    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;
    }
    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

}