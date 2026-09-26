using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BadgeSceneManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float transitionDelay = 1f;
    
    // Internal References
    private SceneChanger sceneChanger;
    
    private void Awake()
    {
        sceneChanger = FindFirstObjectByType<SceneChanger>();
    }

    private void Start()
    {
        StartCoroutine(TriggerTransition());
    }
    
    private IEnumerator TriggerTransition()
    {
        yield return new WaitForSeconds(transitionDelay);
        
        bool beatGame = PlayerState.defeatedPitt && PlayerState.defeatedThelmah;
        if (beatGame)
        {
            sceneChanger.ChangeSceneToCreditsScreen();
        }
        else
        {
            sceneChanger.ChangeSceneToMainMenu();
        }
    }
}
