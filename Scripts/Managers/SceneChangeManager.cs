using EasyTransition;
using UnityEngine;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    [SerializeField] private TransitionSettings transitionSettings;
    
    public void ChangeScene(SceneEnum sceneId)
    {
        bool playCutscene = EvaluateCutScene();
        if (playCutscene)
            sceneId = SceneEnum.BadgeScreen;    

        TransitionManager.Instance().Transition((int) sceneId, transitionSettings, 0f);
    }
    
    public bool EvaluateCutScene()
    {
        bool playCutscene = (PlayerState.defeatedPitt && !PlayerState.defeatedPittCutscenePlayed) ||
                            (PlayerState.defeatedThelmah && !PlayerState.defeatedThelmahCutscenePlayed);

        return playCutscene;
    }

}
