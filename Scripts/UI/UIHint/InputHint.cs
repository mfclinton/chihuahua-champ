using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

[Serializable]
public class InputHintEntry
{
    public RectTransform downStateParent;
    public RectTransform upStateParent;
    public TextMeshProUGUI[] hintTexts { get; private set; }
    public bool isInDownState { get; private set; }
    
    public void Initialize()
    {
        hintTexts = downStateParent.GetComponentsInChildren<TextMeshProUGUI>(true)
            .Concat(upStateParent.GetComponentsInChildren<TextMeshProUGUI>(true))
            .ToArray();
    }
    
    public void SetHintText(string text)
    {
        if (hintTexts == null || hintTexts.Length == 0)
            return;
        
        foreach (TextMeshProUGUI hintText in hintTexts)
            hintText.text = text;
    }
    
    public void SetHintState(bool isInDownState)
    {
        this.isInDownState = isInDownState;
        downStateParent.gameObject.SetActive(isInDownState);
        upStateParent.gameObject.SetActive(!isInDownState);
    }
}

public class InputHint : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private bool isSequentialDownState;
    
    [Header("Hint States")]
    [SerializeField] private RectTransform hintParent;
    [SerializeField] private InputHintEntry[] uiHintEntries;
    
    [Header("Hint Blink Settings")]
    [SerializeField] private float blinkInterval = 0.1f;
    
    [Header("Pop Animation Settings")]
    [SerializeField] private float popDuration = 1.5f;
    [SerializeField] private Ease popEase = Ease.OutBack;
    
    // Internal Variables
    private Tween hintVisibilityTween;
    private Coroutine hintBlinkCoroutine;
    private int currentSeqDownStateIndex;


    // Internal State
    private bool isHintVisible;

    private void Awake()
    {
        if (uiHintEntries.Length == 0)
            Debug.LogError("UIHint: No UIHintEntries found!");
        
        Initialize();
    }

    private void Initialize()
    {
        foreach (InputHintEntry uiHintEntry in uiHintEntries)
            uiHintEntry.Initialize();
        
        // Hide Self by Default
        SetHintVisibility(false);
        hintVisibilityTween.Complete();
    }

    #region State Update Methods

    void UpdateHintState()
    {
        if (isSequentialDownState && 1 < uiHintEntries.Length)
        {
            uiHintEntries[currentSeqDownStateIndex].SetHintState(false);
            currentSeqDownStateIndex = (currentSeqDownStateIndex + 1) % uiHintEntries.Length;
            uiHintEntries[currentSeqDownStateIndex].SetHintState(true);
        }
        else
        {
            // Toggle Hint States
            bool state = !uiHintEntries[0].isInDownState;
            foreach (InputHintEntry uiHintEntry in uiHintEntries)
                uiHintEntry.SetHintState(state);
        }
    }
    
    void ResetHintState()
    {
        currentSeqDownStateIndex = uiHintEntries.Length - 1;

        foreach (InputHintEntry uiHintEntry in uiHintEntries)
            uiHintEntry.SetHintState(false);
    }
    
    public void SetHintBlink(bool isBlinking)
    {
        if (hintBlinkCoroutine != null)
        {
            StopCoroutine(hintBlinkCoroutine);
            hintBlinkCoroutine = null;
        }
        if (isBlinking)
            hintBlinkCoroutine = StartCoroutine(HintBlinkRoutine());
    }

    public void SetHintVisibility(bool isVisible)
    {
        hintVisibilityTween?.Kill(complete: true);

        float targetScale = isVisible ? 1 : 0;

        hintVisibilityTween = hintParent.DOScale(targetScale, popDuration).SetEase(popEase).OnComplete(() =>
        {
            SetHintBlink(isVisible);
            isHintVisible = isVisible;

            if (!isVisible)
                ResetHintState();
        });
    }
    
    public void SetHintText(params string[] texts)
    {
        if (texts.Length == 1)
            SetUniformHintText(texts[0]);
        else
            SetIndividualHintTexts(texts);
    }
    
    public void SetHintTextTransition (params string[] texts)
    {
        if (!isHintVisible)
        {
            SetHintText(texts);
            return;
        }
        
        SetHintVisibility(false);
        hintVisibilityTween.onComplete += () =>
        {
            SetHintText(texts);
            SetHintVisibility(true);
        };
    }
    
    public void Configure(bool isSequentialDownState)
    {
        ResetHintState();
        this.isSequentialDownState = isSequentialDownState;
    }

    #endregion

    #region Helpers

    private void SetUniformHintText(string text)
    {
        foreach (var entry in uiHintEntries)
            entry.SetHintText(text);
    }

    private void SetIndividualHintTexts(params string[] texts)
    {
        for (int i = 0; i < texts.Length; i++)
            uiHintEntries[i].SetHintText(texts[i]);
    }
    
    private IEnumerator HintBlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);
            UpdateHintState();
        }
    }

    #endregion
}
