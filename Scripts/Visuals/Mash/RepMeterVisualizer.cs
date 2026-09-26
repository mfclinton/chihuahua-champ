using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RepMeterVisualizer : ImageFillUpdater
{
    private MashProgressManager mashProgressManager;

    private RectTransform rectTransform;

    protected override void Awake()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            fillImage = GetComponent<Image>();
        }
        
        // Get References
        mashProgressManager = FindAnyObjectByType<MashProgressManager>();
        
        // Initialize Fill Image
        fillImage.fillAmount = 0;
    }

    private void OnEnable()
    {
        mashProgressManager.OnProgressUpdated += UpdateProgress;
        mashProgressManager.OnProgressComplete += Cooldown;
    }
    
    private void OnDisable()
    {
        mashProgressManager.OnProgressUpdated -= UpdateProgress;
        mashProgressManager.OnProgressComplete -= Cooldown;
    }

    // Commented out because it was an unnecessary extra function
    //private void OnProgressUpdated(float progress, ProgressType progressType)
    //{
    //    UpdateProgress(progress, progressType);
    //}

    protected override void UpdateProgress(float targetFillAmount, ProgressType progressType)
    {
        if (fillImage == null)
            return;

        float currentFillAmount = targetFillAmount;

        switch (progressType)
        {
            case ProgressType.KeyMash:
                if (mashProgressManager.IsOnCooldown)
                    return;

                fillImage.fillAmount = currentFillAmount;

                rectTransform.DOComplete();
                rectTransform.localScale = Vector3.one;
                rectTransform.DOPunchScale(new Vector3(0.07f, 0.07f, 0.07f), 0.1f, 1, 0);
                break;
            case ProgressType.Depletion:
                if (mashProgressManager.IsOnCooldown)
                    return;

                fillImage.fillAmount = currentFillAmount;
                break;
            case ProgressType.Cooldown:

                fillImage.DOComplete();
                fillImage.DOFillAmount(0, mashProgressManager.cooldownDuration);
                break;
        }
    }

    private void Cooldown()
    {
        Debug.Log("Setting Rep Meter to 0%");
        UpdateProgress(0, ProgressType.Cooldown);
    }
}