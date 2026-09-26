using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DogMashVisualizer : MonoBehaviour
{
    [Header("Joint Visualizers")]
    [SerializeField] private DogJointVisualizerEntry[] jointVisualizerEntries;

    [Header("Sprite References")]
    [SerializeField] private SpriteRenderer dogFaceSpriteRenderer;
    [SerializeField] private Sprite dogFacedDefault;
    [SerializeField] private Sprite dogFaceHappy;
    [SerializeField] private Sprite dogFaceSad;
    
    [Header("Animation Settings")]
    [SerializeField] private float emoteDuration = 2f;
    [SerializeField] private float completeDuration = .1f;
    
    // Internal References
    private MashGameManager mashGameManager;
    private MashProgressManager mashProgressManager;
    
    // Coroutines
    private Coroutine emoteCoroutine;
    private Coroutine completeCoroutine;
    
    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        // Get References
        mashGameManager = FindFirstObjectByType<MashGameManager>();
        mashProgressManager = FindFirstObjectByType<MashProgressManager>();
        
        // Initialize Joint Visualizers
        foreach (DogJointVisualizerEntry jointVisualizerEntry in jointVisualizerEntries)
            jointVisualizerEntry.Initialize();
        
        // Set Default Dog Face
        dogFaceSpriteRenderer.sprite = dogFacedDefault;
    }

    private void OnEnable()
    {
        mashGameManager.OnGameOver += OnGameOver;
        mashProgressManager.OnProgressUpdated += OnProgressUpdated;
        mashProgressManager.OnProgressComplete += OnProgressComplete;
    }
    
    private void OnDisable()
    {
        mashProgressManager.OnProgressUpdated -= OnProgressUpdated;
        mashProgressManager.OnProgressComplete -= OnProgressComplete;
    }

    private void OnGameOver()
    {
        dogFaceSpriteRenderer.sprite = dogFaceSad;
    }
    
    private void UpdateJointVisualizers(float progress)
    {
        foreach (DogJointVisualizerEntry jointVisualizerEntry in jointVisualizerEntries)
            jointVisualizerEntry.UpdateJoinTransform(progress);
    }
    
    private void OnProgressUpdated(float progress, ProgressType progressType)
    {
        if (mashProgressManager.IsOnCooldown)
            return;
            
        UpdateJointVisualizers(progress);
    }
    
    private IEnumerator CompleteAndReset()
    {
        float resetDuration = mashProgressManager.cooldownDuration - completeDuration;
        
        float timeElapsed = 0f;
        while (timeElapsed < completeDuration)
        {
            timeElapsed += Time.deltaTime;
            UpdateJointVisualizers(1f);
            yield return null;
        }
        
        timeElapsed = 0f;
        while (timeElapsed < resetDuration)
        {
            timeElapsed += Time.deltaTime;
            UpdateJointVisualizers(0f);
            yield return null;
        }
    }
    
    private void OnProgressComplete()
    {
        TriggerEmoteCoroutine(dogFaceHappy);
        TriggerCompleteCoroutine();
    }
    
    private void TriggerCompleteCoroutine()
    {
        if (completeCoroutine != null)
            StopCoroutine(completeCoroutine);
        
        completeCoroutine = StartCoroutine(CompleteAndReset());
    }

    #region Emote Helpers

    private void TriggerEmoteCoroutine(Sprite emoteSprite)
    {
        if (emoteCoroutine != null)
            StopCoroutine(emoteCoroutine);
        
        emoteCoroutine = StartCoroutine(FlashEmote(emoteSprite));
    }

    private IEnumerator FlashEmote(Sprite emoteSprite)
    {
        dogFaceSpriteRenderer.sprite = emoteSprite;
        yield return new WaitForSeconds(emoteDuration);
        dogFaceSpriteRenderer.sprite = dogFacedDefault;
    }

    #endregion
}
