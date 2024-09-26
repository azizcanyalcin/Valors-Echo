using UnityEngine;
using DG.Tweening;

public class PoppingEffect : MonoBehaviour
{
    private Tween poppingTween;
    private void OnEnable()
    {
        StartPoppingEffect();
    }
    private void OnDisable()
    {
        StopPoppingEffect();
    }
    public void StartPoppingEffect()
    {
        RectTransform letterRectTransform = GetComponent<RectTransform>();

        if (letterRectTransform != null)
        {
            poppingTween = letterRectTransform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            Debug.LogWarning("No RectTransform component found on this GameObject!");
        }
    }

    public void StopPoppingEffect()
    {
        if (poppingTween != null && poppingTween.IsActive())
        {
            poppingTween.Kill();
            poppingTween = null;
        }
    }
}
