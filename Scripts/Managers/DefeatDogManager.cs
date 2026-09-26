using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class DefeatDogManager : MonoBehaviour
{   
    [Header("Reference")]
    [SerializeField] private Animator dogAnimator;
    [SerializeField] private TextMeshProUGUI victoryText;
    
    [Header("Config")]
    [SerializeField] private float scaleDuration = 0.5f;
    
    // Internal References
    private MashGameManager mashGameManager;

    // Internal Variables
    private bool dogDefeated;
    private CompetitionMode competitionMode;
    private Vector2 victoryTextOriginalScale;

    private void OnEnable()
    {
        mashGameManager = FindFirstObjectByType<MashGameManager>();

        mashGameManager.OnNumRepsUpdated += CheckRepRequirements;
        mashGameManager.OnNumRepsUpdated += UpdateVictoryText;
        mashGameManager.OnGameOver += DefeatDog;
    }

    private void OnDisable()
    {
        mashGameManager.OnNumRepsUpdated -= CheckRepRequirements;
        mashGameManager.OnNumRepsUpdated -= UpdateVictoryText;
        mashGameManager.OnGameOver -= DefeatDog;
    }

    private void Start()
    {
        dogDefeated = false;
        competitionMode = GameManager.Instance.Mode as CompetitionMode;
        victoryTextOriginalScale = victoryText.rectTransform.localScale;
        
        UpdateVictoryText(0);
    }

    private void CheckRepRequirements(int repCount)
    {
        if(competitionMode != null)
        {
            if (repCount >= competitionMode.liftingRequirements.MinimumReps)
            {
                dogDefeated = true;
                dogAnimator.SetTrigger("Defeat");
            }
        }
        else
        {
            Debug.Log("Invalid CompetitionMode!");
        }
    }

    private void DefeatDog()
    {
        if (dogDefeated == false)
            return;

        switch(competitionMode.dog)
        {
            case (Dog.Pitt):
                PlayerState.defeatedPitt = true;
                break;

            case (Dog.Thelma):
                PlayerState.defeatedThelmah = true;
                break;
        }

        Debug.Log(competitionMode.dog + " defeated!");
        dogAnimator.SetTrigger("Defeat");
    }
    
    private void UpdateVictoryText(int repCount)
    {
        int repsRemaining = competitionMode.liftingRequirements.MinimumReps - repCount;
        if (dogDefeated)
        {
            victoryText.text = "Victory!";
        }
        else
        {
            victoryText.text = $"{repsRemaining} Reps Remaining";
        }
        
        // Scale text to 0 and back
        bool animate = repCount != 0;
        if (animate)
        {
            victoryText.rectTransform.DOScale(Vector3.zero, scaleDuration / 2f).OnComplete(() =>
            {
                victoryText.rectTransform.DOScale(victoryTextOriginalScale, scaleDuration / 2f);
            });
        }
    }
}
