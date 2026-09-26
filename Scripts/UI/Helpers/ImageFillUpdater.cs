using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ImageFillUpdater : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] protected Image fillImage;

    [Header("Visual Settings")]
    [Tooltip("Time it takes to fill the image from 0 to 1")]
    [SerializeField] protected float timeToFill = 1f;

    // Internal Variables
    protected Tweener fillTween;

    protected virtual void Awake()
    {
        if (fillImage == null)
            fillImage = GetComponent<Image>();
    }

    protected virtual void UpdateProgress(float targetFillAmount, ProgressType progressType)
    {
        if (fillImage == null)
            return;

        float currentFillAmount = fillImage.fillAmount;
        float fillDiff = Mathf.Abs(targetFillAmount - currentFillAmount);
        float duration = timeToFill * fillDiff;

        if (fillTween != null && fillTween.IsActive())
            fillTween.Kill();

        fillTween = fillImage.DOFillAmount(targetFillAmount, duration).SetEase(Ease.Linear);
    }
}