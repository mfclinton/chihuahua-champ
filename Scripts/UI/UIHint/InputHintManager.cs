using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHintManager : MonoBehaviour
{
    [Header("UI Hint References")]
    [SerializeField] private InputHint keysUIHint;

    [SerializeField] private InputHint spaceUIHint;

    [SerializeField] private TextMeshProUGUI helperText;

    private void Awake()
    {
        // Event Subscriptions
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        playerController.OnMashKeysRandomized += SetMashKeysUIHint;
        
        MashGameManager mashGameManager = FindFirstObjectByType<MashGameManager>();
        mashGameManager.OnGameOver += OnGameOver;
        
        MashProgressManager mashProgressManager = FindFirstObjectByType<MashProgressManager>();
        mashProgressManager.OnMashTypeFilterChanged += OnMashTypeFilterChanged;
    }

    private void Start()
    {
        // Initial State
        helperText.text = GameManager.Instance.Mode.helperText;
    }

    private void OnGameOver()
    {
        keysUIHint.SetHintVisibility(false);
        spaceUIHint.SetHintVisibility(false);
    }

    private void SetMashKeysUIHint(Key leftKey, Key rightKey)
    {
        Debug.Log($"Setting Mash Keys UI Hint: {leftKey} and {rightKey}");
        string leftKeyText = leftKey.ToString().ToUpper();
        string rightKeyText = rightKey.ToString().ToUpper();
        keysUIHint.SetHintTextTransition(leftKeyText, rightKeyText);
    }
    
    private void OnMashTypeFilterChanged(MashType mashTypeFilter)
    {
        keysUIHint.Configure(mashTypeFilter == MashType.Consecutive);
        
        bool isDefault = mashTypeFilter == MashType.Default;
        spaceUIHint.SetHintVisibility(isDefault);
        keysUIHint.SetHintVisibility(!isDefault);
    }
}
