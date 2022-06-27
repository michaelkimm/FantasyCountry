using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Stage : MonoBehaviour
{
    [SerializeField]
    protected MotorController motorController;

    [SerializeField]
    protected SeatController seatController;

    [SerializeField]
    protected ItemGenerator itemGenerator;

    [SerializeField]
    protected NPCGenerator nPCGenerator;

    [SerializeField]
    protected FixablePart fixablePart;

    [SerializeField]
    protected InputManager inputManager;

    [SerializeField]
    protected FixAbility fixAbility;

    [SerializeField]
    protected Deadable deadable;

    // : >> 스테이지 내 UI
    [SerializeField]
    protected IngameUIController ingameUIController;

    [SerializeField]
    GameObject stageClearUI;

    [SerializeField]
    GameObject stageFailUI;

    protected bool isStageClearUIInvoked = false;

    virtual protected void OnStageClear()
    {
        stageClearUI.SetActive(true);
        IsStageOn = false;
        isClear = true;

        // 별 & 텍스트 정보 업데이트
        JudgeStarAcquired();
        stageClearUI.GetComponent<StageClearUI>()?.SetResult(star);

        // 클리어 소리
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(false);
        SoundManager.Instance.BgmSoundPlayer.PlaySound("BGM_FL_LevelClear");
    }

    virtual protected void OnStageFail()
    {
        stageFailUI.SetActive(true);
        IsStageOn = false;
        isClear = false;

        // 실패 소리
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(false);
        SoundManager.Instance.BgmSoundPlayer.PlaySound("BGM_FL_LevelFailed");
    }
    // <<

    // : >> 스테이지 기록 상태
    [SerializeField]
    protected int stageNum;
    protected int targetNPCCount;
    int currentNPCCount;
    List<RidePart.RidePartType> ridePartTypes = new List<RidePart.RidePartType>();

    [SerializeField]
    protected int[] currentAcquiredRidePartCount;
    [SerializeField]
    protected int[] ridePartCount;

    bool isClear = false;
    [SerializeField]
    int star = 0;
    // <<

    // 스테이지 상태
    [SerializeField]
    protected float stageTime = 180;
    float stagePassedTime = 0;
    void UpdateTimeLeftText()
    {
        ingameUIController.timeLeftText.text = (int)(stageTime / 60f) + " : " + (int)(stageTime % 60);
    }

    protected bool isStageOn = false;
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

    protected bool isStageStop = false;
    virtual public bool IsStageStop
    {
        get => isStageStop;
        set
        {
            isStageStop = value;
            if (isStageStop)
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

    // 감시당할 플레이어
    PlayerController player;
    public UnityEvent stageStart = new UnityEvent();
    public UnityEvent stageEnd = new UnityEvent();

    public UnityEvent stageReplay = new UnityEvent();
    public UnityEvent stageStop = new UnityEvent();
    // 감시당할 의자들?
    // 감시당할 NPC들?

    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
            throw new System.Exception("PlayerController doesnt exist in the scene!");

        if (motorController == null)
            throw new System.Exception("Stage doesnt have motorController!");

        if (ingameUIController == null)
            throw new System.Exception("Stage doesnt have ingameUIController!");

        if (stageClearUI == null)
            throw new System.Exception("Stage doesnt have stageClearUI!");

        if (seatController == null)
            throw new System.Exception("Stage doesnt have seatController!");

        if (nPCGenerator == null)
            throw new System.Exception("Stage doesnt have nPCGenerator!");

        if (itemGenerator == null)
            throw new System.Exception("Stage doesnt have itemGenerator!");

        if (fixablePart == null)
            throw new System.Exception("Stage doesnt have fixablePart!");

        if (inputManager == null)
            throw new System.Exception("Stage doesnt have inputManager!");

        if (fixAbility == null)
            throw new System.Exception("Stage doesnt have fixAbility!");

        if (deadable == null)
            throw new System.Exception("Stage doesnt have deadable!");
    }

    protected virtual void OnEnable()
    {
        // 스테이지 관리자로부터 스테이지 상태 받아온다.
        stageNum = StageManager.Instance.CurrentStage;
        StageRecord stageState = StageManager.Instance.GetStageRecord(stageNum);

        IsStageOn = true;

        // 목표 설정
        UpdateObjective();
    }

    protected virtual void OnDisable()
    {
        // 스테이지 관리자에게 스테이지 상태 돌려준다.
        StageManager.Instance.SetStageState(stageNum, isClear, star);

        // 나중에 고쳐!
        GameManager.Instance.savedNPCCount += currentNPCCount;
        GameManager.Instance.fixedRideCount += 1;

        IsStageOn = false;
    }

    protected virtual void Start()
    {
        // 플레이어에게 잃어버린 부품 알려주기(습득 시, 사운드 위해)
        fixAbility.LetKnowMissingPart(ridePartCount);

        // 획득한 아이템 전송 이벤트 등록
        fixAbility.OnItemGet.AddListener(UpdateCurrentObjectiveState);

        // 죽음 컴포넌트 등록
        deadable.tellDead.AddListener(Failed);

        // 아이템 발생기 초기화
        itemGenerator.InitializeGeneratableItem(ridePartCount);

        // 고쳐지면 스테이지에 알림
        fixablePart.OnFixed.AddListener(AnnounceStageClearConditionFit);

        // 잃어버린 부품 초기화
        fixablePart.Initialized(ridePartCount);

        // 푸쉬버튼 활성화
        ActivatePushButton();

        // 게임 시작과 끝에 실행될 이벤트 등록
        stageStart.AddListener(player.OnGamePlay);
        stageEnd.AddListener(player.OnGameEnd);
        seatController.OnToldOneSit.AddListener(OnSitCountIncrease);

        // 게임 멈춤 & 재생에 실행될 이벤트 등록
        stageStop.AddListener(inputManager.MakeUnavailable);                // 플레이어 인풋 불가
        stageStop.AddListener(itemGenerator.StopGeneratingItemWithRoutine); // 아이템 생성 멈춤
        stageStop.AddListener(motorController.DeactivateMotor);             // 모터 정지

        stageReplay.AddListener(inputManager.MakeAvailable);
        stageReplay.AddListener(itemGenerator.GenerateItemWithRoutine);
        stageReplay.AddListener(motorController.ActivateMotor);
    }

    protected virtual void Update()
    {
        // 게임 실행
        if (!IsStageOn)
            return;

        // 게임 멈춤
        if (IsStageStop)
            return;

        // 타임 UI 업데이트
        UpdateTimeLeftText();

        // 타임 업데이트, 게임 진행 여부 결정
        if (stageTime > 0)
            stageTime -= Time.deltaTime;
        else
        {
            stageTime = 0;
            OnStageFail();
        }
    }

    void ActivatePushButton()
    {
        switch (stageNum)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                ingameUIController.inputUIController.ActivatePushBtn(false);
                break;
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
                ingameUIController.inputUIController.ActivatePushBtn(true);
                break;
        }
    }

    protected abstract IEnumerator ActivateStageStartUI();

    protected abstract void InitializeGameContents();

    public void Stop(float delayTime)
    {
        Invoke("StopStageDelay", delayTime);
    }

    void StopStageDelay() => IsStageStop = true;

    public void Play(float delayTime)
    {
        Invoke("ReplayStageDelay", delayTime);
    }

    void ReplayStageDelay() => IsStageStop = false;

    protected virtual void OnSitCountIncrease()
    {
        if (!IsStageOn)
            return;

        currentNPCCount++;
        UpdateCurrentObjectiveState();
    }

    virtual public void AnnounceStageClearConditionFit()
    {
        if (!isStageClearUIInvoked)
        {
            fixablePart.IsFixed = true;
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

    //public bool CheckRidePartObjective(RidePart.RidePartType rp)
    //{
    //    return ridePartCount[(int)rp] > 0;
    //}

    virtual public void UpdateCurrentObjectiveState(int[] currentAcquiredRidePartCnt_ = null)
    {
        if (currentAcquiredRidePartCnt_ != null)
        {
            for (int i = 0; i < currentAcquiredRidePartCount.Length; i++)
            {
                currentAcquiredRidePartCount[i] = currentAcquiredRidePartCnt_[i];
            }
        }
        ingameUIController.stageObjectiveUIController.SetCurrentObjectiveState(currentAcquiredRidePartCount[0], currentAcquiredRidePartCount[1], currentAcquiredRidePartCount[2], currentNPCCount);
    }

    protected virtual void UpdateObjective()
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
        ingameUIController.stageObjectiveUIController.SetStageObjective(ridePartCount[(int)RidePart.RidePartType.Ratchet], ridePartCount[(int)RidePart.RidePartType.Bolt], ridePartCount[(int)RidePart.RidePartType.Nut], targetNPCCount);
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
            default:
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

                // 50퍼센트 미만
                if (Mathf.Abs(currentNPCCount / (float)targetNPCCount) < 0.5f && !(Mathf.Abs(currentNPCCount / (float)targetNPCCount - 0.5f) < EPSILON))
                    star = 1;
                // 100 퍼센트
                else if (Mathf.Abs(currentNPCCount / (float)targetNPCCount - 1.0f) < EPSILON)
                    star = 3;
                // 50~99퍼센트
                else
                    star = 2;
                break;
        }
    }
}
