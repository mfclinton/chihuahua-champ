using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CompetitionMode", order = 1)]
public class CompetitionMode : GameMode
{
    [Header("Competitions")]

    [Tooltip("The weight and Minimum Reps in order to defeat the competition.")]
    public LiftingRequirements liftingRequirements;

    [Tooltip("The dog to be faced in this competition.")]
    public Dog dog;
}
