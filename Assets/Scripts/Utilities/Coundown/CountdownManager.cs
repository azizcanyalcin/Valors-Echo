using UnityEngine;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    public float countdownTime = 900f;
    public bool isCountdownActive = false;
    public static CountdownManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isCountdownActive)
        {
            countdownTime -= Time.deltaTime;

            if (countdownTime <= 0)
            {
                CountdownFinished();
            }
        }
    }

    public void StartCountdown()
    {
        isCountdownActive = true;
    }

    public void StopCountdown()
    {
        isCountdownActive = false;
    }

    public void ResetCountdown(float newTime = 900f)
    {
        countdownTime = newTime;
        isCountdownActive = false;
    }

    private void CountdownFinished()
    {
        isCountdownActive = false;
        Debug.Log("Countdown finished! Trigger scene change or other actions.");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public string GetFormattedTime()
    {
        return FormatTime(countdownTime);
    }
}
