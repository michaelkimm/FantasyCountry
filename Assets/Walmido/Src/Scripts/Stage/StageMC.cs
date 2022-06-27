using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageMC : MonoBehaviour
{
    [SerializeField]
    SeatController seatController;

    [SerializeField]
    ItemGenerator itemGenerator;

    [SerializeField]
    NPCGenerator nPCGenerator;

    [SerializeField]
    FixablePart fixablePart;

    // : >> 스테이지 내 UI
    [SerializeField]
    TextMeshProUGUI timeLeftText;
    [SerializeField]
    GameObject stageClearUI;
    [SerializeField]
    GameObject stageFailUI;
    [SerializeField]
    StageObjectiveUI stageObjectiveUI;

    [SerializeField]
    BGMSoundPlayer bGMSoundPlayer;

    bool isStageClearUIInvoked = false;

    void OnStageClear()
    {
        stageClearUI.SetActive(true);
        IsStageOn = false;
        isClear = true;

        // 별 & 텍스트 정보 업데이트
        JudgeStarAcquired();
        stageClearUI.GetComponent<StageClearUI>()?.SetResult(star);

        // 클리어 소리
        bGMSoundPlayer.PlaySound("BGM_FL_LevelClear");
    }

    void OnStageFail()
    {
        stageFailUI.SetActive(true);
        IsStageOn = false;
        isClear = false;

        // 클리어 소리
        bGMSoundPlayer.PlaySound("BGM_FL_LevelFailed");
    }
    // <<

    // : >> 스테이지 기록 상태
    int stageNum;
    int targetNPCCount;
    int currentNPCCount;
    List<RidePart.RidePartType> ridePartTypes = new List<RidePart.RidePartType>();
    int[] currentAcquiredRidePartCount;
    int[] ridePartCount;

    bool isClear = false;
    int star = 0;
    // <<

     // 스테이지 상태
    [SerializeField]
    float stageTime = 180;
    float stagePassedTime = 0;
    void UpdateTimeLeftText()
    {
        timeLeftText.text = (int)(stageTime / 60f) + " : " + (int)(stageTime % 60);
    }

    bool isStageOn = false;
    public bool IsStageOn
    {
        get => isStageOn;
        private set
        {
            isStageOn = value;
            if (isStageOn)
                stageStart.Invoke();
            else
                stageEnd.Invoke();
        }
    }

    // 감시당할 플레이어
    PlayerController player;
    public UnityEvent stageStart = new UnityEvent();
    public UnityEvent stageEnd = new UnityEvent();
    // 감시당할 의자들?
    // 감시당할 NPC들?

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
            throw new System.Exception("PlayerController doesnt exist in the scene!");

        if (timeLeftText == null)
            throw new System.Exception("StageMC doesnt have timeLeftText!");

        if (stageClearUI == null)
            throw new System.Exception("StageMC doesnt have stageClearUI!");

        if (stageObjectiveUI == null)
            throw new System.Exception("StageMC doesnt have stageObjectiveUI!");

        if (seatController == null)
            throw new System.Exception("StageMC doesnt have seatController!");

        if (nPCGenerator == null)
            throw new System.Exception("StageMC doesnt have nPCGenerator!");

        if (itemGenerator == null)
            throw new System.Exception("StageMC doesnt have itemGenerator!");

        if (fixablePart == null)
            throw new System.Exception("StageMC doesnt have fixablePart!");

        if (bGMSoundPlayer == null)
            throw new System.Exception("StageMC doesnt have bGMSoundPlayer!");
    }

    void OnEnable()
    {
        // 스테이지 관리자로부터 스테이지 상태 받아온다.
        stageNum = StageManager.Instance.CurrentStage;
        StageRecord stageState = StageManager.Instance.GetStageRecord(stageNum);

        IsStageOn = true;

        // 목표 설정
        UpdateObjective();
    }

    void OnDisable()
    {
        // 스테이지 관리자에게 스테이지 상태 돌려준다.
        StageManager.Instance.SetStageState(stageNum, isClear, star);

        IsStageOn = false;
    }

    void Start()
    {
        // 게임 시작과 끝에 실행될 이벤트 등록
        stageStart.AddListener(player.OnGamePlay);
        stageEnd.AddListener(player.OnGameEnd);
        seatController.OnToldOneSit.AddListener(OnSitCountIncrease);

        //// NPC 만들기
        //nPCGenerator.GenerateRadomNPC(targetNPCCount);

        //fixablePart.Initialized(ridePartCount);

        //// 자리 만들기
        //seatController.Initialize(targetNPCCount);

        //// 아이템 만들기
        //itemGenerator.GenerateItemWithRoutine();
    }

    void Update()
    {
        // 게임 실행
        if (!IsStageOn)
            return;

        // 타임 UI 업데이트
        UpdateTimeLeftText();

        // 타임 업데이트, 게임 진행 여부 결정
        if (stageTime > 0)
            stageTime -= Time.deltaTime;
        else
        {
            stageTime = 0;
            IsStageOn = false;
        }
    }

    void OnSitCountIncrease()
    {
        currentNPCCount++;
        UpdateCurrentObjectiveState();
    }

    public void AnnounceStageClearConditionFit()
    {
        if (!isStageClearUIInvoked)
        {
            Invoke("OnStageClear", 10f);
            isStageClearUIInvoked = true;
        }
    }

    public void Failed()
    {
        if (IsStageOn)
        {
            CancelInvoke("OnStageClear");
            OnStageFail();
        }
    }

    public bool CheckRidePartObjective(RidePart.RidePartType rp)
    {
        return ridePartCount[(int)rp] > 0;
    }

    public void UpdateCurrentObjectiveState(int[] currentAcquiredRidePartCnt_ = null)
    {
        if (currentAcquiredRidePartCnt_ != null)
        {
            for (int i = 0; i < currentAcquiredRidePartCount.Length; i++)
            {
                currentAcquiredRidePartCount[i] = currentAcquiredRidePartCnt_[i];
            }
        }
        stageObjectiveUI.SetCurrentObjectiveState(currentAcquiredRidePartCount[0], currentAcquiredRidePartCount[1], currentAcquiredRidePartCount[2], currentNPCCount);
    }

    void UpdateObjective()
    {
        // 스테이지 조건 가져오기
        StageConditions stageConditions = StageManager.Instance.GetStageCondition(stageNum);
        targetNPCCount = stageConditions.NPCCount;

        // 스테이지 상태 초기화
        int total = 0;
        ridePartCount = new int[(int)RidePart.RidePartType.Size];                   // 목표 고치기 아이템 갯수 배열
        currentAcquiredRidePartCount = new int[(int)RidePart.RidePartType.Size];    // 현재 고치기 아이템 갯수 배열
        for (int i = 0; i < currentAcquiredRidePartCount.Length; i++)               // 현재 초기화
        {
            currentAcquiredRidePartCount[i] = 0;
        }

        for (int i = 0; i < stageConditions.ridePartTypes.Count; i++)               // 목표 갯수 초기화
        {
            ridePartCount[(int)stageConditions.ridePartTypes[i]] += 1;
            total += 1;
        }

        // UI에 전달
        stageObjectiveUI.SetStageObjective(ridePartCount[(int)RidePart.RidePartType.Ratchet], ridePartCount[(int)RidePart.RidePartType.Bolt], ridePartCount[(int)RidePart.RidePartType.Nut], targetNPCCount);
    }

    

    const float EPSILON = 0.000001f;
    void JudgeStarAcquired()
    {
        switch (stageNum)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                if (isClear)
                    star = 3;
                else
                    star = 0;
                break;
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                if (!isClear)
                {
                    star = 0;
                    break;
                }
                if (targetNPCCount == 0)
                {
                    star = 1;
                    break;
                }

                if (Mathf.Abs(currentNPCCount / targetNPCCount - 0.5f) < EPSILON)
                    star = 1;
                else if (Mathf.Abs(currentNPCCount / targetNPCCount - 1.0f) < EPSILON)
                    star = 3;
                else
                    star = 2;
                break;
            default:
                break;
        }
    }
}
