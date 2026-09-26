using System;
using UnityEngine;

public class MashGameInitializer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform benchPressParent;
    [SerializeField] private Transform deadLiftParent;
    
    // Internal Variables
    private MashGameManager mashGameManager;
    
    private void Awake()
    {
        mashGameManager = FindFirstObjectByType<MashGameManager>();
        
        benchPressParent.gameObject.SetActive(false);
        deadLiftParent.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartGame(GameManager.Instance.GetWeight());
    }

    public void StartGame(int selectedWeight)
    {
        //This works for the weight setter, but not the Competitions
        (Attribute.Stat attributeType, MashType mashType, bool randomizeKeysOnRep) = GetGameConfig();
        mashGameManager.Configure(attributeType, selectedWeight, mashType, randomizeKeysOnRep);
        EnableGameModeObjects();
    }

    private (Attribute.Stat attributeType, MashType mashType, bool randomizeKeysOnRep) GetGameConfig()
    {
        GameMode gameMode = GameManager.Instance.Mode;
        return (gameMode.contextualAttributeType, gameMode.mashType, gameMode.randomizeKeysOnRep);
    }
    
    public void EnableGameModeObjects()
    {
        GameMode gameMode = GameManager.Instance.Mode;

        // this just checks to see if we are benching or deadlifting. We're going to change this.
        bool benchPressActive = gameMode.contextualAttributeType == Attribute.Stat.UpperStrength;
        benchPressParent.gameObject.SetActive(benchPressActive);
        deadLiftParent.gameObject.SetActive(!benchPressActive);
    }
}
