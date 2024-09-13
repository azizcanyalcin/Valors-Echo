using TMPro;
using UnityEngine;

public class PopUpTextFx : MonoBehaviour
{
    private TextMeshPro text;
    [SerializeField] private float speed;
    [SerializeField] private float alphaSpeed;
    [SerializeField] private float fadeOutSpeed;
    [SerializeField] private float lifeTime;
    private float textTimer;

    private void Start() {
        text = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;

        if (textTimer < 0)
        {
            float alpha = text.color.a - alphaSpeed * Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

            if (text.color.a < 50) speed = fadeOutSpeed;

            if(text.color.a <= 0) Destroy(gameObject);
        }
    }
}