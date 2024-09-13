using Cinemachine;
using UnityEngine;

public class PlayerFx : EntityFX
{
    [Header("Clone Flash")]
    [SerializeField] private GameObject cloneFlashImage;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float cloneFlashCooldown;
    private float cloneFlashCooldownTimer;

    [Header("Screen Shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakePowerHit;
    public Vector3 shakePowerHighDamage;
    private CinemachineImpulseSource screenShake;
    [Space]
    [SerializeField] private ParticleSystem dustFx;
    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }
    private void Update()
    {
        cloneFlashCooldownTimer -= Time.deltaTime;
    }

    public void CreateCloneFlashImage()
    {
        if (cloneFlashCooldownTimer < 0)
        {
            cloneFlashCooldownTimer = cloneFlashCooldown;
            GameObject newCloneFlashImage = Instantiate(cloneFlashImage, transform.position, transform.rotation);
            newCloneFlashImage.GetComponent<CloneFlashFx>().InitializeCloneFlash(colorLooseRate, spriteRenderer.sprite);
        }
    }
    public void ScreenShake(Vector3 shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * player.facingDirection, shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }
    public void PlayDustFx()
    {
        if (dustFx) dustFx.Play();
    }
}