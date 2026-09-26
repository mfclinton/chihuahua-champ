using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerState : Singleton<PlayerState>
{
    [Header("Initial Stats")]
    [SerializeField] private int initialUpperStrength = 1;
    [SerializeField] private int initialLowerStrength = 4;
    
    // Achievement Trackers
    public static int maxWeightLifted;
    public static int totalReps;
    public static bool defeatedPitt;
    public static bool defeatedThelmah;
    
    // Cutscene Event Trackers
    public static bool defeatedPittCutscenePlayed;
    public static bool defeatedThelmahCutscenePlayed;
    
    // Attributes
    private List<Attribute> attributes;
    
    protected override void Awake()
    {
        base.Awake();
        
        attributes = new List<Attribute>();
        attributes.Add(new Attribute(Attribute.Stat.UpperStrength, initialUpperStrength));
        attributes.Add(new Attribute(Attribute.Stat.LowerStrength, initialLowerStrength));
    }
    
    public Attribute GetAttribute(Attribute.Stat type) => attributes.Find(x => x.Type == type);
}