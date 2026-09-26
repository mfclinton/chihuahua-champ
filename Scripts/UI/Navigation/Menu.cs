using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class Menu : MonoBehaviour
{
    [Header("Icon Sprites")]
    [SerializeField] private Sprite navIcon;
    [SerializeField] private Sprite navIconHover;
    [SerializeField] private Sprite navIconPressed;
    [SerializeField] private Sprite navIconActive;
    [SerializeField] private Sprite navIconLabel;
    public Sprite NavIcon => navIcon;
    public Sprite NavIconHover => navIconHover;
    public Sprite NavIconPressed => navIconPressed;
    public Sprite NavIconActive => navIconActive;
    public Sprite NavIconLabel => navIconLabel;

    [Header("Config")]
    [SerializeField] private bool toggleCanvasVisibility;
    [SerializeField] private int iconOrder;
    [SerializeField] private BackgroundType backgroundType;
    public int IconOrder => iconOrder;
    public BackgroundType BackgroundType => backgroundType;
    
    
    // Internal References
    private Canvas canvas;
    private BackgroundManager backgroundManager;

    
    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        backgroundManager = FindFirstObjectByType<BackgroundManager>();
    }
    
    
    public void SetActive(bool isActive)
    {
        if (isActive)
            backgroundManager.TransitionBackground(backgroundType, () => SetUIActive(true));
        else
            SetUIActive(false);
    }


    void SetUIActive(bool isActive)
    {
        if (toggleCanvasVisibility)
            canvas.enabled = isActive;
    }
}