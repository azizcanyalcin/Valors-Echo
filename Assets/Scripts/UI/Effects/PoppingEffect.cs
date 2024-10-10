using UnityEngine;
using DG.Tweening;

public class PoppingEffect : MonoBehaviour
{
    private Tween poppingTween;
    [SerializeField] private float scaleX = 1.2f;
    [SerializeField] private float scaleY = 1.2f;
    [SerializeField] private float interval = 0.5f;
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
            poppingTween = letterRectTransform.DOScale(new Vector3(scaleX, scaleY, 1f), interval)
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
