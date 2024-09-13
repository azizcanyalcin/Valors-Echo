using TMPro;
using UnityEngine;

public class StatToolTipUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI statDescription;

    public void ShowStatTooltip(string text)
    {
        statDescription.text = text;
        gameObject.SetActive(true);
    }

    public void HideStatTooltip()
    {
        statDescription.text = "";
        gameObject.SetActive(false);
    }
}