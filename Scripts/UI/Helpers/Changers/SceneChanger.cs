using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(SceneEnum scene)
    {
        AudioManager.Instance.StopAllSFX();
        SceneChangeManager.Instance.ChangeScene(scene);
    }

    #region SceneChanger Implementations

    public void ChangeSceneToIntroAnimation()
    {
        AudioManager.Instance.StopAllMusic();
        ChangeScene(SceneEnum.IntroAnimation);
    }
    
    public void ChangeSceneToTitleScreen()
    {
        AudioManager.Instance.StopAllMusic();
        ChangeScene(SceneEnum.TitleScreen);
    }
    
    public void ChangeSceneToMainMenu()
    {
        ChangeScene(SceneEnum.MainMenu);
    }
    
    public void ChangeSceneToTrainingMode()
    {
        ChangeScene(SceneEnum.TrainingMode);
    }
    
    public void ChangeSceneToCompetitionMode()
    {
        ChangeScene(SceneEnum.CompetitionMode);
    }
    
    public void ChangeSceneToBadgeScreen()
    {
        ChangeScene(SceneEnum.BadgeScreen);
    }
    
    public void ChangeSceneToCreditsScreen()
    {
        ChangeScene(SceneEnum.CreditsScreen);
    }
    
    public void ChangeSceneToGameModeScene()
    {
        ChangeScene(GameManager.Instance.Mode.sceneType);
    }
    
    public void ChangeSceneToExit()
    {
        #if UNITY_WEBGL
            ChangeSceneToTitleScreen();
        #else
            Application.Quit();
        #endif
    }

    #endregion
}
