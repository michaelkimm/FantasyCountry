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

    // : >> �������� �� UI
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

        // �� & �ؽ�Ʈ ���� ������Ʈ
        JudgeStarAcquired();
        stageClearUI.GetComponent<StageClearUI>()?.SetResult(star);

        // Ŭ���� �Ҹ�
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(false);
        SoundManager.Instance.BgmSoundPlayer.PlaySound("BGM_FL_LevelClear");
    }

    virtual protected void OnStageFail()
    {
        stageFailUI.SetActive(true);
        IsStageOn = false;
        isClear = false;

        // ���� �Ҹ�
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(false);
        SoundManager.Instance.BgmSoundPlayer.PlaySound("BGM_FL_LevelFailed");
    }
    // <<

    // : >> �������� ��� ����
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

    // �������� ����
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

    // ���ô��� �÷��̾�
    PlayerController player;
    public UnityEvent stageStart = new UnityEvent();
    public UnityEvent stageEnd = new UnityEvent();

    public UnityEvent stageReplay = new UnityEvent();
    public UnityEvent stageStop = new UnityEvent();
    // ���ô��� ���ڵ�?
    // ���ô��� NPC��?

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
        // �������� �����ڷκ��� �������� ���� �޾ƿ´�.
        stageNum = StageManager.Instance.CurrentStage;
        StageRecord stageState = StageManager.Instance.GetStageRecord(stageNum);

        IsStageOn = true;

        // ��ǥ ����
        UpdateObjective();
    }

    protected virtual void OnDisable()
    {
        // �������� �����ڿ��� �������� ���� �����ش�.
        StageManager.Instance.SetStageState(stageNum, isClear, star);

        // ���߿� ����!
        GameManager.Instance.savedNPCCount += currentNPCCount;
        GameManager.Instance.fixedRideCount += 1;

        IsStageOn = false;
    }

    protected virtual void Start()
    {
        // �÷��̾�� �Ҿ���� ��ǰ �˷��ֱ�(���� ��, ���� ����)
        fixAbility.LetKnowMissingPart(ridePartCount);

        // ȹ���� ������ ���� �̺�Ʈ ���
        fixAbility.OnItemGet.AddListener(UpdateCurrentObjectiveState);

        // ���� ������Ʈ ���
        deadable.tellDead.AddListener(Failed);

        // ������ �߻��� �ʱ�ȭ
        itemGenerator.InitializeGeneratableItem(ridePartCount);

        // �������� ���������� �˸�
        fixablePart.OnFixed.AddListener(AnnounceStageClearConditionFit);

        // �Ҿ���� ��ǰ �ʱ�ȭ
        fixablePart.Initialized(ridePartCount);

        // Ǫ����ư Ȱ��ȭ
        ActivatePushButton();

        // ���� ���۰� ���� ����� �̺�Ʈ ���
        stageStart.AddListener(player.OnGamePlay);
        stageEnd.AddListener(player.OnGameEnd);
        seatController.OnToldOneSit.AddListener(OnSitCountIncrease);

        // ���� ���� & ����� ����� �̺�Ʈ ���
        stageStop.AddListener(inputManager.MakeUnavailable);                // �÷��̾� ��ǲ �Ұ�
        stageStop.AddListener(itemGenerator.StopGeneratingItemWithRoutine); // ������ ���� ����
        stageStop.AddListener(motorController.DeactivateMotor);             // ���� ����

        stageReplay.AddListener(inputManager.MakeAvailable);
        stageReplay.AddListener(itemGenerator.GenerateItemWithRoutine);
        stageReplay.AddListener(motorController.ActivateMotor);
    }

    protected virtual void Update()
    {
        // ���� ����
        if (!IsStageOn)
            return;

        // ���� ����
        if (IsStageStop)
            return;

        // Ÿ�� UI ������Ʈ
        UpdateTimeLeftText();

        // Ÿ�� ������Ʈ, ���� ���� ���� ����
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
        // �������� ���� ��������
        StageConditions stageConditions = StageManager.Instance.GetStageCondition(stageNum);
        targetNPCCount = stageConditions.NPCCount;

        // �������� ���� �ʱ�ȭ
        int total = 0;
        ridePartCount = new int[(int)RidePart.RidePartType.Size];                   // ��ǥ ��ġ�� ������ ���� �迭
        currentAcquiredRidePartCount = new int[(int)RidePart.RidePartType.Size];    // ���� ��ġ�� ������ ���� �迭
        for (int i = 0; i < currentAcquiredRidePartCount.Length; i++)               // ���� �ʱ�ȭ
        {
            currentAcquiredRidePartCount[i] = 0;
        }

        for (int i = 0; i < stageConditions.ridePartTypes.Count; i++)               // ��ǥ ���� �ʱ�ȭ
        {
            ridePartCount[(int)stageConditions.ridePartTypes[i]] += 1;
            total += 1;
        }

        // UI�� ����
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

                // 50�ۼ�Ʈ �̸�
                if (Mathf.Abs(currentNPCCount / (float)targetNPCCount) < 0.5f && !(Mathf.Abs(currentNPCCount / (float)targetNPCCount - 0.5f) < EPSILON))
                    star = 1;
                // 100 �ۼ�Ʈ
                else if (Mathf.Abs(currentNPCCount / (float)targetNPCCount - 1.0f) < EPSILON)
                    star = 3;
                // 50~99�ۼ�Ʈ
                else
                    star = 2;
                break;
        }
    }
}
