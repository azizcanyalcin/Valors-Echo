using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer spriteRenderer;
    [Header("PopUp Text FX")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Flash FX")]
    [SerializeField] private Material hitMaterial;
    private Material originalMaterial;

    [Header("Debuff Colors")]
    [SerializeField] private Color[] frostColors;
    [SerializeField] private Color[] burningColors;
    [SerializeField] private Color[] shockedColors;

    // [Header("Debuff FX")]
    // [SerializeField] private ParticleSystem igniteFx;
    // [SerializeField] private ParticleSystem chillFx;
    // [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;

    private GameObject healthBar;

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        originalMaterial = spriteRenderer.material;
        healthBar = GetComponentInChildren<HealthBarUI>().gameObject;
    }
    
   
    public void CreatePopUpText(string text)
    {
        Vector3 offSet = new(Random.Range(-1,1), Random.Range(3,5));

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + offSet, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = text;
    }
    public void MakeTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            healthBar.SetActive(false);
            spriteRenderer.color = Color.clear;
        }
        else
        {
            healthBar.SetActive(true);
            spriteRenderer.color = Color.white;
        }
    }
    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        //Color currentColor = spriteRenderer.color;
        //spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(.2f);

        //spriteRenderer.color = currentColor;
        spriteRenderer.material = originalMaterial;
    }
    private void RedBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }
    private void BurningColorFx()
    {
        if (spriteRenderer.color != burningColors[0]) spriteRenderer.color = burningColors[0];
        else spriteRenderer.color = burningColors[1];

    }
    private void ShockedColorFx()
    {
        if (spriteRenderer.color != shockedColors[0]) spriteRenderer.color = shockedColors[0];
        else spriteRenderer.color = shockedColors[1];
    }
    private void FrostColorFx()
    {
        if (spriteRenderer.color != frostColors[0]) spriteRenderer.color = frostColors[0];
        else spriteRenderer.color = frostColors[1];
    }


    public void ApplyBurningEffect(float seconds)
    {
        //igniteFx.Play();
        InvokeRepeating("BurningColorFx", 0, .15f);
        Invoke("CancelColorChange", seconds);
    }
    public void ApplyFrostEffect(float seconds)
    {
        //chillFx.Play();
        InvokeRepeating("FrostColorFx", 0, .3f);
        Invoke("CancelColorChange", seconds);
    }
    public void ApplyShockedEffect(float seconds)
    {
        //shockFx.Play();
        InvokeRepeating("ShockedColorFx", 0, .15f);
        Invoke("CancelColorChange", seconds);
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;

        // igniteFx.Stop();
        // chillFx.Stop();
        // shockFx.Stop();

    }
    public void CreateHitFx(Transform target)
    {
        GameObject newHitFx = Instantiate(hitFx, target.position, Quaternion.Euler(0, 0, Random.Range(-90, 90)));
        Destroy(newHitFx, .5f);
    }
    public void CreateCriticalHitFx(Transform target)
    {
        GameObject newHitFx = Instantiate(criticalHitFx, target.position - new Vector3(0.2f, 0), Quaternion.Euler(0, 0, Random.Range(10, 55)));
        newHitFx.transform.localScale = new Vector3(GetComponent<Entity>().facingDirection, 1, 1);
        Destroy(newHitFx, .5f);
    }
}