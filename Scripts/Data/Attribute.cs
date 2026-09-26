using System;
using UnityEngine;

public class Attribute
{
    public enum Stat
    {
        UpperStrength,
        LowerStrength
    }

    // Events
    public event Action<int> OnExpAdded;
    public event Action<int> OnLevelUp;

    // State
    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int TotalExpNeeded => CalculateExpForNextLevel(Level);

    // Properties
    public Stat Type { get; private set; }

    public Attribute(Stat type, int initialLevel = 1)
    {
        Type = type;
        Level = initialLevel;
    }

    public void AddExp(int exp)
    {
        CurrentExp += exp;
        OnExpAdded?.Invoke(exp);

        while (CurrentExp >= TotalExpNeeded)
            LevelUp();
    }

    private void LevelUp()
    {
        Level++;
        CurrentExp -= TotalExpNeeded;
        OnLevelUp?.Invoke(Level);
    }

    private int CalculateExpForNextLevel(int level)
    {
        int expNeeded = Mathf.FloorToInt(1000 * Mathf.Pow(1.4f, level - 1));
        return Mathf.Clamp(expNeeded, 1, int.MaxValue);
    }
}