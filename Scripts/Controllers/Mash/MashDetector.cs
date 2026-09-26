using System;
using UnityEngine;

public class MashDetector
{
    // Config
    private float timeLimitLeftRightMash;
    
    // Events
    public event Action<MashType> OnMashDetected;
    
    // State
    private bool leftMashed;
    private bool rightMashed;
    
    private float lastLeftMashTime;
    private float lastRightMashTime;

    public MashDetector(float timeLimitLeftRightMash)
    {
        this.timeLimitLeftRightMash = timeLimitLeftRightMash;
    }
    
    #region Mash Processing

    private void OnMashStateUpdated()
    {
        if (IsConsecutiveMash())
        {
            OnMashDetected?.Invoke(MashType.Consecutive);
        }
        if (IsSimultaneousMash())
        {
            Debug.Log("Simultaneous Mash Detected");
            OnMashDetected?.Invoke(MashType.Simultaneous);
        }
    }
    
    public void OnLeftMash(bool isMashing)
    {
        lastLeftMashTime = Time.time;
        leftMashed = isMashing;
        OnMashStateUpdated();
        
        if (isMashing)
            OnMashDetected?.Invoke(MashType.Left);
    }

    public void OnRightMash(bool isMashing)
    {
        lastRightMashTime = Time.time;
        rightMashed = isMashing;
        OnMashStateUpdated();
        
        if (isMashing)
            OnMashDetected?.Invoke(MashType.Right);
    }
    
    public void OnDefaultMash(bool isMashing)
    {
        if (isMashing)
            OnMashDetected?.Invoke(MashType.Default);
    }
    
    public void Reset()
    {
        leftMashed = false;
        rightMashed = false;
    }

    #endregion

    #region Mash Validation

    private bool ValidateMashTiming()
    {
        return Mathf.Abs(lastLeftMashTime - lastRightMashTime) <= timeLimitLeftRightMash;
    }
    
    private bool IsConsecutiveMash()
    {
        bool onePressed = leftMashed ^ rightMashed;
        return onePressed && ValidateMashTiming();
    }
    
    private bool IsSimultaneousMash()
    {
        bool bothPressed = leftMashed && rightMashed;
        bool inCloseProximity = Mathf.Abs(lastLeftMashTime - lastRightMashTime) <= 0.07f;
        
        return (bothPressed && inCloseProximity);
    }

    #endregion
}