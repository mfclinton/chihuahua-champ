using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MashController : MonoBehaviour
{
    [Header("Mash Properties")]
    [SerializeField] private float timeLimitLeftRightMash;
    
    // Events
    public event Action<MashType> OnMashPerformed;
    
    // Internal References
    private MashDetector mashDetector;
    
    private void Awake()
    {
        // Subscribe to Player Input Events
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        
        playerController.OnLeftMashAction += OnLeftMashAction;
        playerController.OnRightMashAction += OnRightMashAction;
        playerController.OnDefaultMashAction += OnDefaultMashAction;
        
        // Initialize Mash Detector
        mashDetector = new MashDetector(timeLimitLeftRightMash);
        mashDetector.OnMashDetected += OnMashDetected;
    }

    #region Event Handlers

    private void OnLeftMashAction(bool isMashing)
    {
        mashDetector.OnLeftMash(isMashing);
    }
    
    private void OnRightMashAction(bool isMashing)
    {
        mashDetector.OnRightMash(isMashing);
    }

    private void OnDefaultMashAction(bool isMashing)
    {
        mashDetector.OnDefaultMash(isMashing);
    }
    
    private void OnMashDetected(MashType mashType)
    {
        Debug.Log("Mash Type: " + mashType);
        OnMashPerformed?.Invoke(mashType);
    }

    #endregion
}