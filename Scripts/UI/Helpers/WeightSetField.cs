using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using NUnit.Framework.Constraints;
using UnityEngine.UI;

[Serializable]
public class DifficultyUI
{
    public Sprite icon;
    public float numRepsRequiredUpperBound;
}

public class WeightSetField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private RectTransform numberField;
    [SerializeField] private TextMeshProUGUI hundredsDigit, tensDigit, onesDigit;
    
    [Header("Config")]
    [Range(1f, 1.2f)] public float hoverScale;
    [SerializeField] private int scrollIncrement = 5;
    [SerializeField] private Image difficultyIcon;
    
    [Header("Difficulty UI Config")]
    [SerializeField] private DifficultyUI[] difficultyUIs;
    [SerializeField] private float difficultyChangeShakeDuration = 0.05f;
    [SerializeField] private float difficultyChangeShakeStrength = 5f;
    
    // Internal Variables
    private Vector3 originalNumFieldPos;
    private int selectedWeight = 0;
    private bool isHovered;
    
    // Internal References
    private PlayerController playerController;
    private RectTransform panel;

    #region Initialization

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        panel = GetComponent<RectTransform>();
        
        difficultyUIs = difficultyUIs.OrderBy(x => x.numRepsRequiredUpperBound).ToArray();
        
        originalNumFieldPos = numberField.anchoredPosition;
    }

    private void Start()
    {
        ClampSelectedWeight();
        UpdateDigits();
    }

    private void OnEnable()
    {
        playerController.OnMouseScroll += ScrollSelectedWeight;
        GameManager.Instance.OnGameModeChanged += UpdateDifficultyUI;
    }

    private void OnDisable()
    {
        playerController.OnMouseScroll -= ScrollSelectedWeight;
        GameManager.Instance.OnGameModeChanged -= UpdateDifficultyUI;
    }

    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX("ButtonHover");
        panel.DOScale(new Vector2(hoverScale, hoverScale), 0.2f);
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panel.DOScale(Vector2.one, 0.2f);
        isHovered = false;
    }

    #region Weight Selection Helpers

    private void ClampSelectedWeight()
    {
        selectedWeight = Mathf.Clamp(selectedWeight, 10, 200);
    }

    private void ScrollSelectedWeight(float value)
    {
        if (!isHovered)
            return;
        
        float oldWeight = selectedWeight;
        selectedWeight += (int)(value * scrollIncrement);
        ClampSelectedWeight();
        UpdateDigits();

        if(selectedWeight != oldWeight)
            AudioManager.Instance.PlaySFX(value > 0 ? "ScrollUp" : "ScrollDown");

        //Shakes the numberField panel for every scroll.
        numberField.DOComplete();
        numberField.anchoredPosition = originalNumFieldPos;
        numberField.DOShakeAnchorPos(0.05f, new Vector3(0, 5, 0), 20, 0);
        
        // Update the difficulty UI
        GameMode mode = GameManager.Instance.Mode;
        UpdateDifficultyUI(mode);
    }

    private void UpdateDigits()
    {
        string weightDigits = selectedWeight.ToString();

        while (weightDigits.Length != 3)
            weightDigits = "0" + weightDigits;

        //Update Hundreds
        hundredsDigit.text = $"{weightDigits[0]}";
        //Update Tens
        tensDigit.text = $"{weightDigits[1]}";
        //Update Ones
        onesDigit.text = $"{weightDigits[2]}";
    }

    public int GetSelectedWeight() => selectedWeight;

    #endregion

    private void UpdateDifficultyUI(GameMode mode)
    {
        if (difficultyUIs.Length == 0)
            return;

        Weight weight = new Weight(selectedWeight);
        Sprite newDifficultySprite = GetDifficultyIcon(mode, weight);
        if (difficultyIcon.sprite != newDifficultySprite)
        {
            difficultyIcon.sprite = newDifficultySprite;
            difficultyIcon.rectTransform.DOShakeAnchorPos(difficultyChangeShakeDuration, difficultyChangeShakeStrength);
        }
    }

    public Sprite GetDifficultyIcon(GameMode mode, Weight weight)
    {
        var attribute = PlayerState.Instance.GetAttribute(mode.contextualAttributeType);
        float rpr = Equations.CalcRPR(attribute, weight);
        float numRepsRequired = 1 / rpr;
        
        DifficultyUI difficultyUI = difficultyUIs[0];
        foreach (var difficulty in difficultyUIs)
        {
            difficultyUI = difficulty;
            if (numRepsRequired <= difficulty.numRepsRequiredUpperBound)
                break;
        }
        
        Sprite newDifficultySprite = difficultyUI.icon;
        return newDifficultySprite;
    }
}
