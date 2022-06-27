using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LitJson;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

// �������� ���� ����
public class StageManager: iSaveData
{
    static public StageManager instance_;

    static public StageManager Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = new StageManager();
                instance_.Instantiate();
            }
            return instance_;
        }

    }
    
    public void Instantiate()
    {
        // ������ ���� ���� �ʱ�ȭ
        // �������� ��� �ʱ�ȭ
        for (int i = 0; i < TOTAL_STAGE_CNT + 1; i++)
        {
            stageRecords.Add(new StageRecord(i));
            if (i == 1)
                stageRecords[1].IsLocked = false;
        }
        // �������� ���� �ʱ�ȭ
        for (int i = 0; i < TOTAL_STAGE_CNT + 1; i++)
        {
            stageConditions.Add(new StageConditions(i));
        }

        // ������ �ҷ�����
        LoadJsonFile();
    }

    public void OnDisable()
    {
        // ������ ����
        SaveJsonFile();
    }

    const int TOTAL_STAGE_CNT = 20;
    // �������� ���
    List<StageRecord> stageRecords = new List<StageRecord>();
    public StageRecord GetStageRecord(int stageNum)
    {
        return stageRecords[stageNum];
    }

    // �������� Ŭ���� ����
    List<StageConditions> stageConditions = new List<StageConditions>();
    public StageConditions GetStageCondition(int stageNum)
    {
        return stageConditions[stageNum];
    }

    int stageReached = 1;
    public int StageReached
    {
        get => stageReached;
        set { stageReached = value; }
    }
    int currentStage = 1;
    public int CurrentStage
    {
        get => currentStage;
        set { currentStage = value; }
    }
    bool needToShowEndUI = false;
    public bool NeedToShowEndUI { get => needToShowEndUI; set { needToShowEndUI = value; } }

    //public StageManager()
    //{
    //    InitializeData();
    //}

    public void StartStage(int stageNum)
    {
        CurrentStage = stageNum;
    }

    public void EndStage(int stageNum)
    {

    }

    public void SetStageState(int stageNum, bool isClear, int star)
    {
        // �Ȳ������� ������Ʈ
        if (!stageRecords[stageNum].IsCleared)
            stageRecords[stageNum].IsCleared = isClear;

        // ���� ���� ���� �� ������ ������Ʈ
        if (star > stageRecords[stageNum].Star)
            stageRecords[stageNum].Star = star;

        // �������� ������ ���� �������� ����
        if (isClear && (stageNum + 1) <= TOTAL_STAGE_CNT)
        {
            stageRecords[(stageNum + 1)].IsLocked = false;
        }

        // ���� UI ��������ϴ°�?
        if (stageNum == 20 && isClear)
        {
            NeedToShowEndUI = true;
        }
    }

    public void SaveJsonFile()
    {
        string saveDataPath = Path.Combine(Application.dataPath + "/Resources/SaveData");
        if (!File.Exists(saveDataPath))
            Directory.CreateDirectory(saveDataPath);

        // �������� ��� ����
        JsonData stageRecordData = JsonMapper.ToJson(stageRecords);
        string path = Path.Combine(Application.dataPath + "/Resources/SaveData/StageRecord.json");
        File.WriteAllText(path, stageRecordData.ToString());

        // �������� ���� ����
        JsonData stageConditionData = JsonMapper.ToJson(stageConditions);
        path = Path.Combine(Application.dataPath + "/Resources/SaveData/StageCondition.json");
        File.WriteAllText(path, stageConditionData.ToString());

    }

    public void LoadJsonFile()
    {
        // �������� ��� �ҷ�����
        string path = Path.Combine(Application.dataPath + "/Resources/SaveData/StageRecord.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            JsonData stageRecordData = JsonMapper.ToObject(jsonData);
            ParseAsStageRecordWith(stageRecordData);
        }

        // �������� ���� �ҷ�����
        path = Path.Combine(Application.dataPath + "/Resources/SaveData/StageCondition.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            JsonData stageConditionData = JsonMapper.ToObject(jsonData);
            ParseAsStageConditionWith(stageConditionData);
        }
        else
            InitializeStageConditionWithSO();
    }

    void InitializeStageConditionWithSO()
    {
        StageSO[] stageSOs = Resources.LoadAll<StageSO>("ScriptableObject");
        for (int i = 0; i < stageSOs.Length; i++)
        {
            stageConditions[stageSOs[i].StageNum].SetUp(stageSOs[i].NPCCount, stageSOs[i].RidePartTypes);
        }
    }

    void ParseAsStageRecordWith(JsonData data)
    {
        for (int i = 0; i < stageRecords.Count; i++)
        {
            stageRecords[i].StageNumber = (int)data[i]["StageNumber"];
            stageRecords[i].IsLocked = (bool)data[i]["IsLocked"];
            stageRecords[i].IsCleared = (bool)data[i]["IsCleared"];
            stageRecords[i].Star = (int)data[i]["Star"];

            if (stageRecords[i].IsLocked == false && stageRecords[i].IsCleared == false)
                stageReached = stageRecords[i].StageNumber;
        }
    }

    void ParseAsStageConditionWith(JsonData data)
    {
        for (int i = 0; i < stageRecords.Count; i++)
        {
            stageConditions[i].StageNum = (int)data[i]["StageNum"];
            stageConditions[i].PlayTime = (int)data[i]["PlayTime"];

            stageConditions[i].NPCCount = (int)data[i]["NPCCount"];

            for (int j = 0; j < data[i]["ridePartTypes"].Count; j++)
            {
                stageConditions[i].AddRidePartType((RidePart.RidePartType)((int)data[i]["ridePartTypes"][j]));
            }
        }
    }
}
