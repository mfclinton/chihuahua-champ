using System;
using System.Collections;
using UnityEngine;

public class MashProgressManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private MashType mashTypeFilter;

    public float cooldownDuration { get; private set; } = 2f;
    
    // Events
    public event Action OnMashSuccess;
    public event Action<float, ProgressType> OnProgressUpdated;
    public event Action OnProgressComplete;
    
    public event Action<MashType> OnMashTypeFilterChanged;
    
    // State
    public float Progress { get; private set; }
    public bool IsOnCooldown => cooldownRoutine != null;

    //barLifted tells us if the player has successfully mashed the first time when the rep meter was at 0, then play an SFX.
    //resets every cooldown.
    private bool barLifted;
    
    // Internal References
    private MashController mashController;
    private Coroutine cooldownRoutine;

    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        // Get References
        mashController = FindAnyObjectByType<MashController>();
        
        // Events
        mashController.OnMashPerformed += OnMashPerformed;
    }

    private void OnMashPerformed(MashType mashType)
    {
        if (mashType != mashTypeFilter || IsOnCooldown)
            return;
        
        OnMashSuccess?.Invoke();
    }
    
    public void SetMashTypeFilter(MashType mashTypeFilter)
    {
        this.mashTypeFilter = mashTypeFilter;
        OnMashTypeFilterChanged?.Invoke(mashTypeFilter);
    }
    
    public void Configure(MashType mashTypeFilter)
    {
        SetMashTypeFilter(mashTypeFilter);
    }

    #region Update Progress Methods

    public void UpdateProgress(float delta, ProgressType progressType)
    {
        if (barLifted == false && !IsOnCooldown && progressType == ProgressType.KeyMash)
        {
            AudioManager.Instance.PlaySFX("BarbellLift");
            barLifted = true;
        }

        Progress = Mathf.Clamp01(Progress + delta);
        OnProgressUpdated?.Invoke(Progress, progressType);
        
        if (1 <= Progress)
            OnProgressCompleteHandler();
    }

    private void OnProgressCompleteHandler()
    {
        AudioManager.Instance.PlaySFX("SmolDogBork");
        Progress = 0;
        StartCooldown();
        OnProgressComplete?.Invoke();
    }

    #endregion
    
    #region Cooldown Methods
    
    public void StartCooldown()
    {
        if (cooldownRoutine != null)
            StopCoroutine(cooldownRoutine);
        
        cooldownRoutine = StartCoroutine(CooldownRoutine());
        barLifted = false;
    }
    
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownDuration);
        cooldownRoutine = null;
    }
    
    #endregion
}
