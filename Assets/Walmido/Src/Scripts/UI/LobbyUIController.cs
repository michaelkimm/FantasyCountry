using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField]
    string story1SceneName = "GyroswingB_stage_live_story1";

    [SerializeField]
    string stageSelect = "StageSelect";

    string sceneName;
    
    public void JumpIntoGame()
    {
        GotoScene(story1SceneName, true);
        SoundManager.Instance.BgmSoundPlayer.PlaySound("");
    }

    public void GotoScene(string name, bool needLoading = false)
    {
        sceneName = name;
        if (needLoading)
            Invoke("ChangeSceneWithLoadingDelay", 1f);
        else
            Invoke("ChangeSceneDelay", 1f);
    }
    void ChangeSceneDelay() => GameManager.Instance.ChangeScene(sceneName);
    void ChangeSceneWithLoadingDelay() => GameManager.Instance.ChangeScene(sceneName, true);

    void Start()
    {
        PlayBGM();
    }

    void PlayBGM()
    {
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(true);
        SoundManager.Instance.BgmSoundPlayer.PlaySound(SoundManager.Instance.GetSceneBGMName(SceneManager.GetActiveScene().name));
    }
}
