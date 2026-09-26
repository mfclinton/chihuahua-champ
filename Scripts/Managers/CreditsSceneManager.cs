using System;
using System.Collections;
using UnityEngine;

public class CreditsSceneManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int numBarks = 3;
    [SerializeField] private float barkDelay = 0.4f;
    [SerializeField] private float transitionDelay = 15f;

    // Internal References
    private SceneChanger sceneChanger;
    
    private void Awake()
    {
        sceneChanger = FindFirstObjectByType<SceneChanger>();
    }

    private void Start()
    {
        TriggerBarks();
        TriggerTransition();
    }

    public void TriggerBarks()
    {
        StartCoroutine(TriggerBarksRoutine());
    }
    
    public void TriggerTransition()
    {
        StartCoroutine(TriggerTransitionRoutine());
    }
    
    private IEnumerator TriggerBarksRoutine()
    {
        for (int i = 0; i < numBarks; i++)
        {
            AudioManager.Instance.PlaySFX("SmolDogBork");
            yield return new WaitForSeconds(barkDelay);
        }
    }
    
    private IEnumerator TriggerTransitionRoutine()
    {
        yield return new WaitForSeconds(transitionDelay);
        
        sceneChanger.ChangeSceneToMainMenu();
    }
}
