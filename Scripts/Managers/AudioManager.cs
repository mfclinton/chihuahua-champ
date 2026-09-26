using System;
using FMODUnity;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class GamemodeMusicEntry
{
    public StudioEventEmitter musicEmitter;

    public GameMode[] contextualGameModes;
}

[Serializable]
public class SFXClipEntry
{
    public StudioEventEmitter sfxEmitter;

    public string clipName;
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("References")]
    [SerializeField] private List<GamemodeMusicEntry> gamemodeMusicEntries;
    [SerializeField] private List<SFXClipEntry> sfxClipEntries;

    protected override void Awake()
    {
        base.Awake();
        
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        gameManager.OnGameModeChanged += OnGameModeChanged;
        
    }

    private void OnDestroy()
    {
        foreach (GamemodeMusicEntry entry in gamemodeMusicEntries)
        {
            if (entry.musicEmitter.IsPlaying())
                entry.musicEmitter.Stop();
        }
    }

    private void OnGameModeChanged(GameMode gameMode)
    {
        AudioManager.Instance.StopAllSFX();
        foreach (GamemodeMusicEntry entry in gamemodeMusicEntries)
        {
            if (entry.contextualGameModes.Contains(gameMode))
            {
                if (!entry.musicEmitter.IsPlaying())
                    entry.musicEmitter.Play();
            }
            else
            {
                if (entry.musicEmitter.IsPlaying())
                    entry.musicEmitter.Stop();
            }
        }
    }

    public void StopAllMusic()
    {
        foreach (GamemodeMusicEntry entry in gamemodeMusicEntries) 
        {
            entry.musicEmitter.Stop();
        }
        Debug.Log("All Music Stopped.");
    }

    public void StopAllSFX()
    {
        foreach (SFXClipEntry entry in sfxClipEntries)
        {
            entry.sfxEmitter.Stop();
        }
        Debug.Log("All SFX Stopped");
    }


    public void PlaySFX(string clipName)
    {
        StudioEventEmitter entry = sfxClipEntries.Find(x => x.clipName == clipName).sfxEmitter;

        if (entry != null)
        {
            if(entry.IsPlaying())
                entry.Stop();

            entry.Play();
        }
    }

    public void PlayClickSFX()
    {
       PlaySFX("ButtonClick");
    }
}
