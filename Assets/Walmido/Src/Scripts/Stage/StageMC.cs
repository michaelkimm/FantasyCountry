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

    // : >> �������� �� UI
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

        // �� & �ؽ�Ʈ ���� ������Ʈ
        JudgeStarAcquired();
        stageClearUI.GetComponent<StageClearUI>()?.SetResult(star);

        // Ŭ���� �Ҹ�
        bGMSoundPlayer.PlaySound("BGM_FL_LevelClear");
    }

    void OnStageFail()
    {
        stageFailUI.SetActive(true);
        IsStageOn = false;
        isClear = false;

        // Ŭ���� �Ҹ�
        bGMSoundPlayer.PlaySound("BGM_FL_LevelFailed");
    }
    // <<

    // : >> �������� ��� ����
    int stageNum;
    int targetNPCCount;
    int currentNPCCount;
    List<RidePart.RidePartType> ridePartTypes = new List<RidePart.RidePartType>();
    int[] currentAcquiredRidePartCount;
    int[] ridePartCount;

    bool isClear = false;
    int star = 0;
    // <<

     // �������� ����
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

    // ���ô��� �÷��̾�
    PlayerController player;
    public UnityEvent stageStart = new UnityEvent();
    public UnityEvent stageEnd = new UnityEvent();
    // ���ô��� ���ڵ�?
    // ���ô��� NPC��?

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
        // �������� �����ڷκ��� �������� ���� �޾ƿ´�.
        stageNum = StageManager.Instance.CurrentStage;
        StageRecord stageState = StageManager.Instance.GetStageRecord(stageNum);

        IsStageOn = true;

        // ��ǥ ����
        UpdateObjective();
    }

    void OnDisable()
    {
        // �������� �����ڿ��� �������� ���� �����ش�.
        StageManager.Instance.SetStageState(stageNum, isClear, star);

        IsStageOn = false;
    }

    void Start()
    {
        // ���� ���۰� ���� ����� �̺�Ʈ ���
        stageStart.AddListener(player.OnGamePlay);
        stageEnd.AddListener(player.OnGameEnd);
        seatController.OnToldOneSit.AddListener(OnSitCountIncrease);

        //// NPC �����
        //nPCGenerator.GenerateRadomNPC(targetNPCCount);

        //fixablePart.Initialized(ridePartCount);

        //// �ڸ� �����
        //seatController.Initialize(targetNPCCount);

        //// ������ �����
        //itemGenerator.GenerateItemWithRoutine();
    }

    void Update()
    {
        // ���� ����
        if (!IsStageOn)
            return;

        // Ÿ�� UI ������Ʈ
        UpdateTimeLeftText();

        // Ÿ�� ������Ʈ, ���� ���� ���� ����
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
