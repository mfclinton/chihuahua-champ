using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float transitionDuration = 1;
    
    // Internal References
    private Camera mainCamera;
    private Background[] backgrounds;
    
    // Internal Variables
    private Tween transitionTween;

    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        // Get References
        mainCamera = Camera.main;
        backgrounds = FindObjectsByType<Background>(FindObjectsSortMode.None);
    }
    
    public void TransitionBackground(BackgroundType type, Action onComplete = null)
    {
        // Get Target Background
        Background targetBackground = backgrounds.FirstOrDefault(b => b.Type == type);
        if (targetBackground == null)
        {
            onComplete?.Invoke();
            return;
        }
        
        if (transitionTween != null)
            transitionTween.Kill();
        
        // Transition Camera
        Vector3 targetPosition = targetBackground.transform.position;
        targetPosition.z = mainCamera.transform.position.z;
        
        transitionTween = mainCamera.transform.DOMove(targetPosition, transitionDuration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
