using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AshController : MonoBehaviour
{
    [SerializeField] private string targetLayer = "Player";
    [SerializeField] private float blindnessTime = 3f;
    [SerializeField] private Rigidbody2D rb;
    private CharacterStats stats;

    private Vignette vignette;
    private Volume volume;

    void Start()
    {
        volume = FindObjectOfType<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0;
        }
    }

    public void SetupAsh(CharacterStats stats)
    {

        this.stats = stats;
    }
    public void OnAshAnimationHit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask(targetLayer));

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer(targetLayer))
            {
                stats.DealPhysicalDamage(collider.GetComponent<CharacterStats>(), 1f);
                StartCoroutine(BlindPlayer());
            }
        }

        OnAshHit();
    }

    private void OnAshHit()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 10f);
    }
    private IEnumerator BlindPlayer()
    {
        float timeElapsed = 0f;
        while (timeElapsed < 0.5)
        {
            vignette.intensity.value = Mathf.Lerp(0, 1, timeElapsed / 0.5f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        vignette.intensity.value = 1;

        yield return new WaitForSeconds(blindnessTime);

        timeElapsed = 0f;
        while (timeElapsed < 0.5f)
        {
            vignette.intensity.value = Mathf.Lerp(1, 0, timeElapsed / 0.5f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        vignette.intensity.value = 0;
    }
}