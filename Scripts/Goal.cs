using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public enum GoalType
    {
        Level,
        Weight,
        Win
    }
    
    [Header("References")]
    [SerializeField] private Image checkBox;
    [SerializeField] private Sprite achievedIcon, unachievedIcon;
    
    [Header("Config")]
    [SerializeField] private GoalType goalType;
    [SerializeField] private int goal;

    private void Start()
    {
        UpdateGoalStatus();
    }

    public void UpdateGoalStatus()
    {
        bool goalAchieved = false;
        if (goalType == GoalType.Level)
        {
            goalAchieved = LevelGoalAchieved();
        }
        else if (goalType == GoalType.Weight)
        {
            goalAchieved = WeightGoalAchieved();
        }
        else if (goalType == GoalType.Win)
        {
            goalAchieved = WinGoalAchieved();
        }
        
        checkBox.sprite = goalAchieved ? achievedIcon : unachievedIcon;
    }
    
    private bool LevelGoalAchieved()
    {
        int levelGoal = goal;
        
        return PlayerState.Instance.GetAttribute(Attribute.Stat.UpperStrength).Level >= levelGoal || PlayerState.Instance.GetAttribute(Attribute.Stat.LowerStrength).Level >= levelGoal;
    }
    
    private bool WeightGoalAchieved()
    {
        int weightGoal = goal;
        
        return PlayerState.maxWeightLifted >= weightGoal;
    }
    
    private bool WinGoalAchieved()
    {
        return PlayerState.defeatedPitt && PlayerState.defeatedThelmah;
    }
}
