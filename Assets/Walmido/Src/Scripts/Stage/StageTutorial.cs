using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageTutorial : Stage
{
    [SerializeField]
    TutorialScriptsController tutorialScriptsController;

    RidePart.RidePartType tutorialItem;
    bool ridePartNeedDestroyDelay = false;
    float ridePartInteractableDelay = 0f;
    bool isTutorial = true;
    public void SetTutorial(bool value) => isTutorial = value;

    public UnityEvent stageTutorialReplay = new UnityEvent();

    public override bool IsStageStop
    {
        get => isStageStop;
        set
        {
            isStageStop = value;

            if (isTutorial)
            {
                if (IsStageStop)
                {
                    stageStop.Invoke();
                    GameManager.Instance.isGameStopAndStoryOn = true;
                }
                else
                {
                    stageTutorialReplay.Invoke();
                    GameManager.Instance.isGameStopAndStoryOn = false;
                }
            }
            else
            {
                if (IsStageStop)
                {
                    stageStop.Invoke();
                    GameManager.Instance.isGameStopAndStoryOn = true;
                }
                else
                {
                    stageReplay.Invoke();
                    GameManager.Instance.isGameStopAndStoryOn = false;
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (tutorialScriptsController == null)
            throw new System.Exception("StageTutorial doesnt have tutorialScriptsController");
    }

    protected override void Start()
    {
        base.Start();
        Stop(0);

        StartCoroutine(ActivateStageStartUI());
    }

    protected override IEnumerator ActivateStageStartUI()
    {
        // 스테이지 스타트 UI 발동
        float delay = ingameUIController.ActivateStageStartUI(stageNum);
        yield return new WaitForSeconds(delay);

        InitializeGameContents();
    }

    protected override void InitializeGameContents()
    {
        // 튜토리얼 첫번째 스크립트 활성화
        tutorialScriptsController.ShowPage(0);

        // 튜토리얼 게임 사운드 실행
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(true);
        SoundManager.Instance.BgmSoundPlayer.PlaySound("BGM_FL_Tuto");

        stageTutorialReplay.AddListener(inputManager.MakeAvailable);
        stageTutorialReplay.AddListener(motorController.ActivateMotor);
    }

    public void SetGenerateItem(int itemIdx)
    {
        tutorialItem = (RidePart.RidePartType)itemIdx;
    }

    public void SetGenerateItemNeedDestroy(bool value)
    {
        ridePartNeedDestroyDelay = value;
    }

    public void SetGenerateItemInteractableDelay(float delay)
    {
        ridePartInteractableDelay = delay;
    }

    public void GenerateItem(float angle)
    {
        itemGenerator.GenerateObject(4f, angle, ridePartNeedDestroyDelay,  3f, tutorialItem, ridePartInteractableDelay);
    }

    public void GenerateNPC()
    {
        nPCGenerator.GenerateObject(5, 300);
    }

    public void GenerateSeat(int seatIdx)
    {
        seatController.EnableSeat(seatIdx);
    }

    public override void UpdateCurrentObjectiveState(int[] currentAcquiredRidePartCnt_ = null)
    {
        base.UpdateCurrentObjectiveState(currentAcquiredRidePartCnt_);

        if (stageNum == 1)
        {
            if (tutorialScriptsController.NextScriptIdx == 7 || tutorialScriptsController.NextScriptIdx == 8)
            {
                tutorialScriptsController.ShowNextPage(0);
                Stop(0);
            }
        }
    }

    protected override void OnSitCountIncrease()
    {
        base.OnSitCountIncrease();

        if (!IsStageOn)
            return;

        if (tutorialScriptsController.NextScriptIdx == 6)
        {
            tutorialScriptsController.ShowNextPage(0);
            Stop(0);
        }
    }

    public override void AnnounceStageClearConditionFit()
    {
        base.AnnounceStageClearConditionFit();
        tutorialScriptsController.ShowNextPage(0);
        if (stageNum == 1)
            Stop(0);
    }

    protected override void OnStageClear()
    {
        base.OnStageClear();
        tutorialScriptsController.StopShowCurrentPage(0);
    }

    protected override void OnStageFail()
    {
        base.OnStageFail();
        tutorialScriptsController.DeactivateAll();

        //tutorialScriptsController.StopShowCurrentPage(0);
    }
}
