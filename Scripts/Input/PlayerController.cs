using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class is responsible for listening to the Player Input and invoking the respective events.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Mash Control Randomization")]
    [SerializeField] private Key[] leftMashRandomizableKeys;
    [SerializeField] private Key[] rightMashRandomizableKeys;
    
    // Rep Input Actions
    public Action<bool> OnLeftMashAction;
    public Action<bool> OnRightMashAction;
    public Action<bool> OnDefaultMashAction;
    
    public Action<Key, Key> OnMashKeysRandomized;
    
    // UI Input Actions
    public Action<Vector2> OnClickAction;
    public Action<float> OnMouseScroll;

    // Internal References
    private GameplayActions gameplayActions;
    
    // Internal Variables
    private ControlRandomizer leftMashControlRandomizer;
    private ControlRandomizer rightMashControlRandomizer;
    
    private void Awake()
    {
        // Creates a Input Actions instance
        gameplayActions = new GameplayActions();
        gameplayActions.Enable();

        
        // Subscribes to Mash Input Events
        SubscribeToMashActions();
        
        // Initializes Mash Input Randomizers
        InitializeRandomizers();

        //Subscribe to UI Input Events
        SubscribeToUIActions();
    }

    private void OnEnable()
    {
        gameplayActions.PlayerActions.Pause.started += ctx => ProcessPauseAction();
    }

    private void OnDisable()
    {
        gameplayActions.PlayerActions.Pause.canceled -= ctx => ProcessPauseAction();
        gameplayActions.Disable();
    }
    
    #region Gameplay Event Helpers
    
    private void ProcessPauseAction()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        gameManager.TogglePauseState();
    }
    
    private void ProcessLeftMashAction(bool isMashing)
    {
        OnLeftMashAction?.Invoke(isMashing);
    }
    
    private void ProcessRightMashAction(bool isMashing)
    {
        OnRightMashAction?.Invoke(isMashing);
    }
    
    private void ProcessDefaultMashAction(bool isMashing)
    {
        OnDefaultMashAction?.Invoke(isMashing);
    }
    
    private void SubscribeToMashActions()
    {
        var playerActions = gameplayActions.PlayerActions;
        
        playerActions.LeftMash.performed += ctx => ProcessLeftMashAction(true);
        playerActions.LeftMash.canceled += ctx => ProcessLeftMashAction(false);
        
        playerActions.RightMash.performed += ctx => ProcessRightMashAction(true);
        playerActions.RightMash.canceled += ctx => ProcessRightMashAction(false);
        
        playerActions.DefaultMash.performed += ctx => ProcessDefaultMashAction(true);
        playerActions.DefaultMash.canceled += ctx => ProcessDefaultMashAction(false);
    }

    #endregion
    
    #region UI Event Helpers
    
    private void SubscribeToUIActions()
    {
        var uiActions = gameplayActions.UI;
        
        uiActions.ScrollWheel.performed += ProcessScrollAction;
        uiActions.Click.performed += ProcessClickAction;
    }
    
    private void ProcessScrollAction(InputAction.CallbackContext context)
    {
        float rawValue = context.ReadValue<Vector2>().y;
        float value = Mathf.Clamp(rawValue, -1, 1);
        OnMouseScroll?.Invoke(value);
    }
    
    private void ProcessClickAction(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton())
            return;
        
        Vector2 position = gameplayActions.UI.Point.ReadValue<Vector2>();
        OnClickAction?.Invoke(position);
    }
    
    #endregion
    
    #region Helpers
    
    private void InitializeRandomizers()
    {
        leftMashControlRandomizer = new ControlRandomizer(gameplayActions.PlayerActions.LeftMash, leftMashRandomizableKeys);
        rightMashControlRandomizer = new ControlRandomizer(gameplayActions.PlayerActions.RightMash, rightMashRandomizableKeys);
    }

    public void RandomizeMashKeys()
    {
        leftMashControlRandomizer.BindRandomKey();
        rightMashControlRandomizer.BindRandomKey();
        OnMashKeysRandomized?.Invoke(leftMashControlRandomizer.RandomKey, rightMashControlRandomizer.RandomKey);
    }

    #endregion
}