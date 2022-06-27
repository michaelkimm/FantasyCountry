using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngameUIController : MonoBehaviour
{
    public TextMeshProUGUI timeLeftText;

    public InputUIController inputUIController;

    public StageObjectiveUI stageObjectiveUIController;

    [SerializeField]
    float stageStartFadeDelay = 0.5f;

    [SerializeField]
    List<StageStartUIController> stageStartUIControllers = new List<StageStartUIController>();

    void Awake()
    {
        if (timeLeftText == null)
            throw new System.Exception("IngameUIController doesn't have timeLeftText");
        if (inputUIController == null)
            throw new System.Exception("IngameUIController doesn't have inputUIController");
        if (stageObjectiveUIController == null)
            throw new System.Exception("IngameUIController doesn't have stageObjectiveUIController");
    }

    // 끝나는 시간 리턴
    public float ActivateStageStartUI(int stageNum)
    {
        stageStartUIControllers[stageNum - 1].gameObject.SetActive(true);
        return stageStartUIControllers[stageNum - 1].ActivateShinny(stageStartFadeDelay);
    }

    public void StopGameProgram()
    {
        GameManager.Instance.StopGame(true);
    }
}
