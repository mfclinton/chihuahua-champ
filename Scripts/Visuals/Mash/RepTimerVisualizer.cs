using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RepTimerVisualizer : ImageFillUpdater
{
    [Header("Config")]
    [SerializeField] private Gradient gradient;

    private RectTransform rectTransform;
    private Vector3 originalRepTimerPos;

    // Internal References
    private MashGameManager mashGameManager;
    private MashProgressManager mashProgressManager;

    protected override void Awake()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            originalRepTimerPos = rectTransform.anchoredPosition;
            fillImage = GetComponent<Image>();
        }

        // Get References
        mashGameManager = FindFirstObjectByType<MashGameManager>();
        mashProgressManager = FindFirstObjectByType<MashProgressManager>();
        
        // Initialize Fill Image
        fillImage.fillAmount = 1;
    }

    private void OnEnable()
    {
        mashGameManager.OnRepTimeRemainingUpdated += OnTimeElapsedRepUpdated;
    }
    
    private void OnDisable()
    {
        mashGameManager.OnRepTimeRemainingUpdated -= OnTimeElapsedRepUpdated;
    }

    private void OnTimeElapsedRepUpdated(float repTimeRemaining, ProgressType progressType)
    {
        float progress = (repTimeRemaining / mashGameManager.TimeToCompleteRep);

        // Set the Fill
        UpdateProgress(progress, progressType);
        
        // Set the Color
        fillImage.color = gradient.Evaluate(1f - progress);
    }

    protected override void UpdateProgress(float targetFillAmount, ProgressType progressType)
    {
        if (fillImage == null)
            return;

        float currentFillAmount = targetFillAmount;

        switch (progressType)
        {
            case ProgressType.Depletion:
                if (mashProgressManager.IsOnCooldown)
                    return;

                fillImage.fillAmount = currentFillAmount;
                break;
            case ProgressType.Cooldown:

                rectTransform.DOComplete();
                rectTransform.anchoredPosition = originalRepTimerPos;
                rectTransform.DOShakeAnchorPos(mashProgressManager.cooldownDuration, new Vector3(3, 3, 3), 100);

                fillImage.DOComplete();
                fillImage.DOFillAmount(currentFillAmount, mashProgressManager.cooldownDuration);
                break;
            default: 
                break;
        }
    }

    //private void Cooldown()
    //{
    //    Debug.Log("Setting Rep Time Limit to 100%");
    //    UpdateProgress(1, ProgressType.Cooldown);
    //}
}
