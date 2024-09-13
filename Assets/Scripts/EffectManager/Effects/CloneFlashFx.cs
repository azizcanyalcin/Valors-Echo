    using UnityEngine;

public class CloneFlashFx : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float colorLooseRate;

    public void InitializeCloneFlash(float colorLooseRate, Sprite sprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        this.colorLooseRate = colorLooseRate;
    }

    private void Update()
    {
        float alpha = spriteRenderer.color.a - colorLooseRate * Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if (spriteRenderer.color.a <= 0) Destroy(gameObject);
    }
}