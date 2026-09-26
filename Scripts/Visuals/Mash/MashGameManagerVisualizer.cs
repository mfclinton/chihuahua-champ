using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MashGameManager))]
public class MashGameManagerVisualizer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform repMeterHUD;
    [SerializeField] private TextMeshProUGUI repsValue;
    
    // Internal References
    private MashGameManager mashGameManager;
    private Attribute displayedPlayerStat;
    
    private void Awake()
    {
        // Events
        mashGameManager = GetComponent<MashGameManager>();
       
        mashGameManager.OnNumRepsUpdated += SetRepsText;
        mashGameManager.OnAttributeTypeUpdated += OnAttributeTypeUpdated;
    }

    private void Start()
    {
        //This sets the rep meter's location based on the game mode
        repMeterHUD.anchoredPosition = GameManager.Instance.Mode.repMeterPosition;
    }

    private void OnAttributeTypeUpdated(Attribute.Stat playerStat)
    {
        if (displayedPlayerStat != null)
        {
            displayedPlayerStat.OnExpAdded -= OnXPAdded;
            displayedPlayerStat.OnLevelUp -= OnLevelUp;
        }
        
        displayedPlayerStat = PlayerState.Instance.GetAttribute(playerStat);
        
        displayedPlayerStat.OnExpAdded += OnXPAdded;
        displayedPlayerStat.OnLevelUp += OnLevelUp;
        
        //SetPlayerStatText(displayedPlayerStat.Level);
    }
    
    private void OnXPAdded(int xp)
    {
        // TODO: Add some visual feedback for XP added
    }
    
    private void OnLevelUp(int level)
    {
        SetPlayerStatText(level);
    }

    #region UI Update Methods

    //private void SetTimerText(float timeElapsed)
    //{
    //    timerText.text = TimeSpan.FromSeconds(timeElapsed).ToString(@"mm\:ss");
    //}
    
    private void SetRepsText(int numReps)
    {
        repsValue.text = numReps.ToString();
    }
    
    private void SetPlayerStatText(int playerStat)
    {
        //playerStatText.text = playerStat.ToString();
    }

    #endregion
}
