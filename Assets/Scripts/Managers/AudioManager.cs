using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private float distanceToSource;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    private bool canPlaySFX;
    public bool playBGM;
    private int bgmIndex;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        Invoke("CanPlaySFX", 1);
    }
    private void Update()
    {
        if (!playBGM) StopAll();
        else if (!bgm[bgmIndex].isPlaying) PlayBGM(bgmIndex);

    }
    public void PlaySFX(int index, Transform source, bool canFadeIn)
    {
        if (!canPlaySFX || index >= sfx.Length)
            return;

        AudioSource audioSource = sfx[index];
        float playerDistance = source != null ? Vector2.Distance(PlayerManager.instance.player.transform.position, source.position) : 0f;

        if (source != null && playerDistance > distanceToSource)
            return;

        audioSource.pitch = Random.Range(.95f, 1.05f);

        if (audioSource.isPlaying)
        {
            StopSFX(index, true);
        }

        if (canFadeIn)
        {
            audioSource.volume = 0;
            StartCoroutine(FadeInSFX(audioSource, 1f, 1f));
        }

        audioSource.Play();
    }

    public void StopSFX(int index, bool canFadeOut)
    {
        if (sfx[index])
        {
            if (canFadeOut) StartCoroutine(FadeOutSFX(sfx[index], 1f));
            else sfx[index].Stop();
        }
    }
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayBGM(int index)
    {
        bgmIndex = index;
        StopAll();
        bgm[bgmIndex].Play();
    }

    public void StopAll()
    {
        foreach (AudioSource bgm in bgm)
        {
            bgm.Stop();
        }
    }

    private void CanPlaySFX()
    {
        canPlaySFX = true;
    }

    private IEnumerator FadeInSFX(AudioSource audioSource, float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
    private IEnumerator FadeOutSFX(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

}
