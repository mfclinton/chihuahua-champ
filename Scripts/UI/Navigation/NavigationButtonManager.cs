using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class NavigationButtonManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button mainButton;
    [SerializeField] private Transform menuButtonsParent;
    
    [Header("Prefabs")]
    [SerializeField] private Button menuButtonPrefab;

    [Header("Settings")]
    [SerializeField] private bool isReversed = true;
    [SerializeField] private float mainButtonTweenDuration = 0.2f;
    [SerializeField] private float iconTweenDuration = 0.2f;
    
    [Header("Main Button Formatting Configuration")]
    [SerializeField] private float mainButtonSizeMultiplierOpen = 1.3f;
    [SerializeField] private float mainButtonSizeMultiplierClosed = 1f;
    
    [Header("Icon List Formatting Configuration")]
    [Range(0, 360)] [SerializeField] private float iconListStart = 180;
    [SerializeField] private float iconListRadius = 5;
    [SerializeField] private float iconListSpacing = 15;
    
    // Internal References
    private Menu[] menus;
    
    // Internal Variables
    private Dictionary<Menu, Button> menuButtons;
    private Vector2 mainButtonDefaultScale;
    
    // State Variables
    private bool isNavigationOpen;
    private Menu activeMenu;
    private Menu prevActiveMenu;
    
    // Constants
    private const string NAVIGATION_TAG = "Navigation";

    #region Initialization

    private void Awake()
    {
        Initialize();
    }


    private void Start()
    {
        // Set Initial State
        SetMenuActive(menus[0]);
        SetNavigationState(false);
    }


    private void Initialize()
    {
        // Get References
        menus = FindObjectsByType<Menu>(FindObjectsSortMode.None).OrderBy(m => m.IconOrder).ToArray();
        
        // Set Variables
        mainButtonDefaultScale = mainButton.image.rectTransform.localScale;
        
        // Initialize Buttons
        mainButton.tag = NAVIGATION_TAG;
        InitializeMenuButtons();
        
        // Handle Events
        mainButton.onClick.AddListener(OnMainButtonClicked);
        
        // Subscribe to Pointer Click Event
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        playerController.OnClickAction += OnPointerClick;
    }
    
    
    private void InitializeMenuButtons()
    {
        menuButtons = new Dictionary<Menu, Button>();
        
        foreach (Menu menu in menus)
        {
            Button menuButton = Instantiate(menuButtonPrefab, menuButtonsParent);
            menuButton.tag = NAVIGATION_TAG;
            
            // Set the icon sprite highlighted, pressed, selected, disabled
            menuButton.image.sprite = menu.NavIcon;
            menuButton.spriteState = new SpriteState
            {
                highlightedSprite = menu.NavIconHover,
                pressedSprite = menu.NavIconPressed
            };
            
            
            menuButton.onClick.AddListener(() => OnMenuButtonClicked(menu));
            menuButtons.Add(menu, menuButton);
        }
    }
    
    
    #endregion

    #region Public Methods

    public void GoToPreviousMenu()
    {
        SetMenuActive(prevActiveMenu);
    }

    #endregion
    
    #region Event Callbacks
    

    private void OnMainButtonClicked()
    {
        string navWheelSFX = isNavigationOpen ? "NavWheelContract" : "NavWheelExpand";
        AudioManager.Instance.PlaySFX(navWheelSFX);
        SetNavigationState(!isNavigationOpen);
    }
    
    
    private void OnMenuButtonClicked(Menu menu)
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        SetMenuActive(menu);
    }
    
    
    private void OnPointerClick(Vector2 position)
    {
        if (!isNavigationOpen)
            return;
        
        // Check if the pointer is over a tagged object
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(NAVIGATION_TAG))
                return;
        }
        
        SetNavigationState(false);
    }

    #endregion

    #region Main Button Helpers

    private void UpdateMainButtonImage()
    {
        mainButton.image.sprite = isNavigationOpen ? activeMenu.NavIconLabel : activeMenu.NavIcon;
    }

    
    private void UpdateMainButtonSize()
    {
        float targetSizeMultiplier = isNavigationOpen ? mainButtonSizeMultiplierOpen : mainButtonSizeMultiplierClosed;
        mainButton.image.rectTransform.DOScale(mainButtonDefaultScale * targetSizeMultiplier, mainButtonTweenDuration);
    }

    #endregion
    
    #region Icon List Helpers

    private void SetNavigationState(bool isOpen)
    {
        isNavigationOpen = isOpen;
        
        // Update Icon Positions
        UpdateMenuButtonPositions();
        
        // Update Main Button Icon
        UpdateMainButtonImage();
        
        UpdateMainButtonSize();
    }
    

    private void SetMenuActive(Menu menu)
    {
        if (activeMenu == menu)
            return;
        
        foreach (Menu otherMenu in menus)
        {
            otherMenu.SetActive(otherMenu == menu);
            menuButtons[otherMenu].image.sprite = otherMenu == menu ? otherMenu.NavIconActive : otherMenu.NavIcon;
        }
        
        prevActiveMenu = activeMenu;
        activeMenu = menu;
        
        // Update Main Button Icon
        UpdateMainButtonImage();
    }
    
    
    public Sequence UpdateMenuButtonPositions()
    {
        Sequence menuButtonSequence = DOTween.Sequence();
        for (int i = 0; i < menus.Length; i++)
        {
            Menu menu = menus[i];
            Button menuButton = menuButtons[menu];

            Vector2 goalPosition = Vector2.zero;
            if (isNavigationOpen)
                goalPosition = GetIconCirclePosition(i);

            menuButtonSequence.Join(menuButton.image.rectTransform.DOAnchorPos(goalPosition, iconTweenDuration));
        }
        
        return menuButtonSequence;
    }

    
    private Vector2 GetIconCirclePosition(int index)
    {
        float currentAngle = iconListStart + index * iconListSpacing;
        if (isReversed)
            currentAngle = -currentAngle;
        
        float radianAngle = currentAngle * Mathf.Deg2Rad;
        Vector2 circlePos = new Vector2(
            Mathf.Cos(radianAngle) * iconListRadius, 
            Mathf.Sin(radianAngle) * iconListRadius
        );

        return circlePos;
    }

    #endregion
}
