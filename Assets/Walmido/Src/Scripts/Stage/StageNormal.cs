using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNormal : Stage
{
    bool didSoundChanged = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(ActivateStageStartUI());
    }

    protected override IEnumerator ActivateStageStartUI()
    {
        // 스테이지 스타트 UI 발동
        float delay = ingameUIController.ActivateStageStartUI(stageNum);
        Stop(0);
        yield return new WaitForSeconds(delay);
        Play(0);

        InitializeGameContents();
    }

    protected override void InitializeGameContents()
    {
        // 일반 게임 사운드 실행
        SoundManager.Instance.BgmSoundPlayer.PlaySound("BGM_FL_Ingame");

        // NPC 만들기
        nPCGenerator.GenerateRadomNPC(targetNPCCount);

        // 자리 만들기
        seatController.Initialize(targetNPCCount);

        // 아이템 만들기
        itemGenerator.GenerateItemWithRoutine();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            print("Force stage clear");
            AnnounceStageClearConditionFit();
            currentAcquiredRidePartCount[0] = 10;
            currentAcquiredRidePartCount[1] = 10;
            currentAcquiredRidePartCount[2] = 10;
            UpdateCurrentObjectiveState();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            stageTime = 100;
        }

        if (stageTime <= 98)
        {
            if (didSoundChanged)
                return;
            SoundManager.Instance.BgmSoundPlayer.PlaySound("BGM_FL_Ingame_burn");
            didSoundChanged = true;
        }
    }
}
