using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MashGameOverVisualizer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas gameOverCanvas;
    
    [SerializeField] private TextMeshProUGUI beforePlayerStatText;
    [SerializeField] private Image statArrowImage;
    [SerializeField] private TextMeshProUGUI afterPlayerStatText;
    [SerializeField] private TextMeshProUGUI repsCompletedText;
    [SerializeField] private TextMeshProUGUI weightLiftedText;
    [SerializeField] private TextMeshProUGUI timeElapsedText;
    
    // Internal References
    private MashGameManager mashGameManager;

    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        // Get References
        mashGameManager = FindFirstObjectByType<MashGameManager>();
        
        mashGameManager.OnGameOver += OnGameOver;
        
        mashGameManager.OnTimeElapsedUpdated += SetTimeElapsed;
        mashGameManager.OnNumRepsUpdated += SetRepsCompleted;
        mashGameManager.OnSelectedWeightUpdated += SetWeightLifted;
        mashGameManager.OnXPProcessed += SetPlayerStats;
    }

    #region Event Callbacks

    private void OnGameOver()
    {
        gameOverCanvas.enabled = true;
        AudioManager.Instance.PlaySFX("RoundOver");
    }

    private void SetPlayerStats(int beforeLevel, int afterLevel)
    {
        beforePlayerStatText.text = beforeLevel.ToString();
        if (beforeLevel != afterLevel)
        {
            // DG Tween the scale to 1 for arrow and before
            statArrowImage.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            beforePlayerStatText.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
        }
        
        afterPlayerStatText.text = afterLevel.ToString();
    }
    
    private void SetRepsCompleted(int numReps)
    {
        repsCompletedText.text = numReps.ToString();
    }
    
    private void SetWeightLifted(int weightLifted)
    {
        weightLiftedText.text = weightLifted.ToString();
    }
    
    private void SetTimeElapsed(float timeElapsed)
    {
        timeElapsedText.text = TimeSpan.FromSeconds(timeElapsed).ToString(@"mm\:ss");
    }

    #endregion
}
