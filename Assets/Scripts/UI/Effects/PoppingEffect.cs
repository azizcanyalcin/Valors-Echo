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
        Transform targetTransform = GetComponent<RectTransform>() ?? transform;

        if (targetTransform != null)
        {
            poppingTween = targetTransform.DOScale(new Vector3(scaleX, scaleY, 1f), interval)
                .SetLoops(-1, LoopType.Yoyo);
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
