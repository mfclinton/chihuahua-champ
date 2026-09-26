using UnityEngine;

public class GameModeChanger : MonoBehaviour
{
    public void ChangeGameMode(GameModeEnums gameModeType, bool changeScene = true)
    {
        // Get the GameMode
        GameMode gameMode = GameManager.Instance.GetGameMode(gameModeType);
        GameManager.Instance.ChangeGameMode(gameMode);
        
        // Change the scene if necessary
        if (changeScene)
            SceneChangeManager.Instance.ChangeScene(gameMode.sceneType);
    }

    #region Change GameMode Implementations

    public void ChangeGameModeToMenu(bool changeScene = true)
    {
        ChangeGameMode(GameModeEnums.Menu, changeScene);
    }
    
    public void ChangeGameModeToBP_Training(bool changeScene = true)
    {
        ChangeGameMode(GameModeEnums.BP_Training, changeScene);
    }
    
    public void ChangeGameModeToBP_Competition(bool changeScene = true)
    {
        ChangeGameMode(GameModeEnums.BP_Competition, changeScene);
    }
    
    public void ChangeGameModeToDL_Training(bool changeScene = true)
    {
        ChangeGameMode(GameModeEnums.DL_Training, changeScene);
    }
    
    public void ChangeGameModeToDL_Competition(bool changeScene = true)
    {
        ChangeGameMode(GameModeEnums.DL_Competition, changeScene);
    }

    #endregion
}
