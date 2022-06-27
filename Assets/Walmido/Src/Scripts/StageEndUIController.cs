using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEndUIController : MonoBehaviour
{
    [SerializeField]
    string tutorialSceneName = "GyroswingB_stage_live_tutorial";

    [SerializeField]
    string storySceneName = "GyroswingB_stage_live_story";

    [SerializeField]
    string normalSceneName = "GyroswingB_stage_live";

    public void GotoStageSelect()
    {
        GameManager.Instance.ChangeScene("StageSelect");
    }

    public void RestartStage()
    {
        GameManager.Instance.ReloadCurrentScene();
    }

    public void GotoNextStage()
    {
        StageManager.Instance.CurrentStage = StageManager.Instance.CurrentStage + 1;

        // 현재 씬이 튜토리얼 1일 경우
        if (StageManager.Instance.CurrentStage == 2 || StageManager.Instance.CurrentStage == 7)
        {
            GameManager.Instance.ChangeScene(normalSceneName);
        }
        // 다음 씬이 튜토리얼이면
        else if (StageManager.Instance.CurrentStage == 6)
        {
            GameManager.Instance.ChangeScene(storySceneName + StageManager.Instance.CurrentStage.ToString());
        }
        else if(StageManager.Instance.CurrentStage == 21)
        {
            StageManager.Instance.CurrentStage = 20;
            // StageManager.Instance.NeedToShowEndUI = true;
            GameManager.Instance.ChangeScene("GyroswingB_stage_live_story end");
        }
        else
            GameManager.Instance.ReloadCurrentScene();

    }
}
