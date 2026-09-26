using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<GameMode> gameModes;

    // Events
    public event Action<GameMode> OnGameModeChanged;
    public static Action<bool> OnPauseStateChanged;

    // State Variables
    public static bool gamePaused = false;
    public static bool usingKilograms = true;
    public static int selectedWeight = -1;
    public GameMode Mode { get; private set; }
    
    // Highscore Variables
    private int highestBenchPressCompleted = 100;
    private int highestDeadliftCompleted = 100;

    protected override void Awake()
    {
        base.Awake();

        Mode = gameModes[0];
    }

    public void TogglePauseState()
    {
        gamePaused = !gamePaused;
        OnPauseStateChanged?.Invoke(gamePaused);
    }

    #region Gamemode Methods

    public GameMode GetGameMode(GameModeEnums gameModeType) => gameModes.Find(x => x.gameModeType == gameModeType);
    
    public void ChangeGameMode(GameMode gameMode)
    {
        Mode = gameMode;
        OnGameModeChanged?.Invoke(Mode);
    }
    
    #endregion

    #region Highscore Methods

    public void NewHighestBenchPressWeight(int newValue) => highestBenchPressCompleted = newValue;

    public void NewHighestDeadliftWeight(int newValue) => highestDeadliftCompleted = newValue;

    public int GetHighestBenchPressCompleted() => highestBenchPressCompleted;

    public int GetHighestDeadliftCompleted() => highestDeadliftCompleted;
    
    #endregion

    public int GetWeight()
    {
        if (Mode is CompetitionMode)
        {
            return (int) ((CompetitionMode) Mode).liftingRequirements.weightValue;
        }
        else
        {
            return selectedWeight;
        }
    }
}
