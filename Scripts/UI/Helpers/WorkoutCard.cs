using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutCard : MonoBehaviour
{
    public bool isAvailable;

    private Image cardImage;

    [Header("Card References")]
    [SerializeField] private Image difficultyImage;
    
    [Header("Config")]
    [SerializeField] private CompetitionMode competitionMode;
    [SerializeField]
    private Sprite availableCard, unavailableCard;

    [SerializeField]
    private List<GameObject> children;

    private void Start()
    {
        cardImage = GetComponent<Image>();

        ToggleCard(isAvailable);
        UpdateDifficultyImage();
    }

    public void ToggleCard(bool availability)
    {
        isAvailable = availability;

        cardImage.sprite = isAvailable ? availableCard : unavailableCard;

        foreach (GameObject child in children)
            child.SetActive(isAvailable);
    }
    
    void UpdateDifficultyImage()
    {
        if (competitionMode == null)
            return;
        
        if (competitionMode == null)
        {
            difficultyImage.gameObject.SetActive(false);
            return;
        }
        
        WeightSetField weightSetField = FindFirstObjectByType<WeightSetField>();
        Weight weight = new Weight(competitionMode.liftingRequirements.weightValue);
        difficultyImage.sprite = weightSetField.GetDifficultyIcon(competitionMode, weight);
    }
}
