using System;
using UnityEngine;

public class MashGameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeToCompleteRep = 5f;
    public float TimeToCompleteRep => timeToCompleteRep;
    
    // Internal References
    private PlayerController playerController;
    private MashProgressManager mashProgressManager;
    private Attribute.Stat attributeType;
    private Attribute attr => PlayerState.Instance.GetAttribute(attributeType);
    
    // Events
    public event Action<float> OnTimeElapsedUpdated;
    public event Action<float, ProgressType> OnRepTimeRemainingUpdated; 
    
    public event Action<int> OnNumRepsUpdated;
    public event Action<int> OnSelectedWeightUpdated;
    public event Action<Attribute.Stat> OnAttributeTypeUpdated;
    
    public event Action OnGameOver;
    public event Action<int, int> OnXPProcessed;
    
    // Internal Variables
    private bool randomizeKeysOnRep;

    private Weight selectedWgt = new Weight();
    
    private int numReps;
    private float timeElapsed;
    private float repTimeRemaining;
    
    // State
    private bool isPaused;

    
    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        mashProgressManager = FindFirstObjectByType<MashProgressManager>();

        repTimeRemaining = timeToCompleteRep;
        isPaused = true;
    }

    private void OnEnable()
    {
        mashProgressManager.OnMashSuccess += ProcessMashSuccess;
        mashProgressManager.OnProgressComplete += OnProgressComplete;
    }

    private void OnDisable()
    {
        mashProgressManager.OnMashSuccess -= ProcessMashSuccess;
        mashProgressManager.OnProgressComplete -= OnProgressComplete;
    }

    private void Update()
    {
        ProcessTimestep();
    }
    
    public void SetAttributeType(Attribute.Stat attributeType)
    {
        this.attributeType = attributeType;
        OnAttributeTypeUpdated?.Invoke(attributeType);
    }
    
    public void SetSelectedWeight(int selectedWeight)
    {
        selectedWgt.SetValue(selectedWeight);
        OnSelectedWeightUpdated?.Invoke((int)selectedWgt.value);
    }
    
    private void UpdateTimeElapsed(float timeElapsed)
    {
        this.timeElapsed = timeElapsed;
        OnTimeElapsedUpdated?.Invoke(timeElapsed);
    }
    
    private void UpdateRepTimeRemaining(float repTimeRemaining, ProgressType progressType)
    {
        this.repTimeRemaining = repTimeRemaining;
        OnRepTimeRemainingUpdated?.Invoke(this.repTimeRemaining, progressType);
        
        if (repTimeRemaining <= 0)
            GameOver();
    }

    private void GameOver()
    {
        isPaused = true;
        OnGameOver?.Invoke();

        AudioManager.Instance.PlaySFX("DogPantingFast");
        HandleXP();
    }

    public void Configure(Attribute.Stat attributeType, int selectedWeight, MashType mashTypeFilter, bool randomizeKeysOnRep)
    {
        SetAttributeType(attributeType);
        SetSelectedWeight(selectedWeight);
        
        mashProgressManager.Configure(mashTypeFilter);
        
        this.randomizeKeysOnRep = randomizeKeysOnRep;
        playerController.RandomizeMashKeys();
        
        // TODO: better pause
        isPaused = false;
    }
    
    private void ProcessTimestep()
    {
        if (isPaused)
            return;
        
        float delta = GetMashProgressTimeStep();
        mashProgressManager.UpdateProgress(delta, ProgressType.Depletion);
        
        UpdateTimeElapsed(timeElapsed + Time.deltaTime);

        if (mashProgressManager.IsOnCooldown)
            return;
        // The Rep Time Limit does NOT deplete when on cooldown
        UpdateRepTimeRemaining(repTimeRemaining - Time.deltaTime, ProgressType.Depletion);
    }

    private void ProcessMashSuccess()
    {
        if (isPaused)
            return;
        
        float value = GetMashProgressActionStep();
        mashProgressManager.UpdateProgress(value, ProgressType.KeyMash);
        Debug.Log($"Mash Success! Value: {value}");
    }
    
    private void OnProgressComplete()
    {
        PerformRep();
    }
    
    private void PerformRep()
    {
        numReps++;
        OnNumRepsUpdated?.Invoke(numReps);
        
        PlayerState.maxWeightLifted = Mathf.Max(PlayerState.maxWeightLifted, (int)selectedWgt.value);
        PlayerState.totalReps++;
        
        UpdateRepTimeRemaining(timeToCompleteRep, ProgressType.Cooldown);
        
        if (randomizeKeysOnRep)
            playerController.RandomizeMashKeys();
    }

    #region Mash Progress Calculation

    private float GetMashProgressActionStep()
    {
        return Equations.CalcRPR(attr, selectedWgt);
    }

    private float GetMashProgressTimeStep()
    {
        return -Equations.CalcPWIR(attr, selectedWgt, timeElapsed) * Time.deltaTime;
    }

    #endregion

    // This currently takes the current level, displays it on results along with the new level if there is a level up.
    private void HandleXP()
    {
        int beforeLevel = attr.Level;
        
        int expGained = numReps * selectedWgt.expYield;
        attr.AddExp(expGained);
        
        int afterLevel = attr.Level;
        OnXPProcessed?.Invoke(beforeLevel, afterLevel);

    }
}
