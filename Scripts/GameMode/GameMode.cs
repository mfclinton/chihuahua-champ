using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewGameMode", menuName = "ScriptableObjects/GameMode", order = 1)]
public class GameMode : ScriptableObject
{
    [FormerlySerializedAs("gamemodeType")]
    [Header("General")]
    
    [Tooltip("The type of game mode")]
    public GameModeEnums gameModeType;
    
    [Tooltip("The scene associated with this game mode")]
    public SceneEnum sceneType;
    
    [Tooltip("Which of the player's attributes is being applied in this game mode?")]
    public Attribute.Stat contextualAttributeType;

    [Header("Mash Functional Settings")]
    
    [Tooltip("In what way will the player have to mash their keys?")]
    public MashType mashType;

    [Tooltip("Will there be a new set of random keys to mash every rep?")]
    public bool randomizeKeysOnRep;
    
    [Header("Mash Visual Settings")]
    [Tooltip("The spawn position of the Rep Meter")]
    public Vector3 repMeterPosition;

    [Tooltip("The message that will display above the Input Hint")]
    public string helperText;
    
    [Tooltip("The characters that will be spawned for this game mode, accompanied with their spawn locations")]
    public SerializableDictionary<GameObject, Vector3> charactersToSpawn;
}
