using DG.Tweening;
using UnityEngine;

public class InteractableIcon : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 0.2f;

    void Start()
    {
        Sequence seq = DOTween.Sequence();
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = Vector3.one * scaleFactor;

        seq.Append(transform.DOScale(targetScale, speed))
            .Append(transform.DOShakeRotation(shakeDuration, new Vector3(0, 0, shakeStrength * 20f)))
            .Append(transform.DOScale(originalScale, speed))
            .SetLoops(-1);
    }
}
