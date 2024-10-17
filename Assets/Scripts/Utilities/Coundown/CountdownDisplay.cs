using UnityEngine;
using TMPro;

public class CountdownDisplay : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    private void Update()
    {
        if (CountdownManager.instance != null && countdownText != null)
        {
            countdownText.text = CountdownManager.instance.GetFormattedTime();
        }
    }
}
