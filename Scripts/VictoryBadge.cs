using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class VictoryBadge : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float lockedAlpha = 0.15f;
    [SerializeField] private float unlockedAlpha = 1f;
    [SerializeField] private Dog dog;
    
    [Header("Animation Config")]
    [SerializeField] private bool animateOnUnlock = true;
    [SerializeField] private float animationDuration = 5f;
    [SerializeField] private float animationDelay = 1f;
    
    // Internal References
    private Image image;
    
    // Animation Variables
    
    
    private void Awake()
    {
        image = GetComponent<Image>();
        
        UpdateState();
    }

    public void UpdateState()
    {
        bool unlocked = false;
        bool cutScenePlayed = false;

        switch (dog)
        {
            case Dog.Pitt:
                unlocked = PlayerState.defeatedPitt;
                cutScenePlayed = PlayerState.defeatedPittCutscenePlayed;
                break;
            case Dog.Thelma:
                unlocked = PlayerState.defeatedThelmah;
                cutScenePlayed = PlayerState.defeatedThelmahCutscenePlayed;
                break;
        }

        bool playCutScene = unlocked && !cutScenePlayed;
        if (playCutScene)
        {
            switch (dog)
            {
                case Dog.Pitt:
                    PlayerState.defeatedPittCutscenePlayed = true;
                    break;
                case Dog.Thelma:
                    PlayerState.defeatedThelmahCutscenePlayed = true;
                    break;
            }
        }

        if (animateOnUnlock && playCutScene)
        {
            SetLocked(false);
            image.DOFade(unlockedAlpha, animationDuration).SetDelay(animationDelay);
        }
        else
        {
            SetLocked(unlocked);
        }
    }
    
    public void SetLocked(bool unlocked)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, unlocked ? unlockedAlpha : lockedAlpha);
    }
}
