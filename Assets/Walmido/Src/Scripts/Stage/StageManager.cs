using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LitJson;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

// 스테이지 상태 관리
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
        // 데이터 저장 공간 초기화
        // 스테이지 기록 초기화
        for (int i = 0; i < TOTAL_STAGE_CNT + 1; i++)
        {
            stageRecords.Add(new StageRecord(i));
            if (i == 1)
                stageRecords[1].IsLocked = false;
        }
        // 스테이지 조건 초기화
        for (int i = 0; i < TOTAL_STAGE_CNT + 1; i++)
        {
            stageConditions.Add(new StageConditions(i));
        }

        // 데이터 불러오기
        LoadJsonFile();
    }

    public void OnDisable()
    {
        // 데이터 저장
        SaveJsonFile();
    }

    const int TOTAL_STAGE_CNT = 20;
    // 스테이지 기록
    List<StageRecord> stageRecords = new List<StageRecord>();
    public StageRecord GetStageRecord(int stageNum)
    {
        return stageRecords[stageNum];
    }

    // 스테이지 클리어 조건
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
        // 안깻었으면 업데이트
        if (!stageRecords[stageNum].IsCleared)
            stageRecords[stageNum].IsCleared = isClear;

        // 새로 얻은 별이 더 많으면 업데이트
        if (star > stageRecords[stageNum].Star)
            stageRecords[stageNum].Star = star;

        // 스테이지 깻으면 다음 스테이지 오픈
        if (isClear && (stageNum + 1) <= TOTAL_STAGE_CNT)
        {
            stageRecords[(stageNum + 1)].IsLocked = false;
        }

        // 엔드 UI 보여줘야하는가?
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

        // 스테이지 기록 저장
        JsonData stageRecordData = JsonMapper.ToJson(stageRecords);
        string path = Path.Combine(Application.dataPath + "/Resources/SaveData/StageRecord.json");
        File.WriteAllText(path, stageRecordData.ToString());

        // 스테이지 조건 저장
        JsonData stageConditionData = JsonMapper.ToJson(stageConditions);
        path = Path.Combine(Application.dataPath + "/Resources/SaveData/StageCondition.json");
        File.WriteAllText(path, stageConditionData.ToString());

    }

    public void LoadJsonFile()
    {
        // 스테이지 기록 불러오기
        string path = Path.Combine(Application.dataPath + "/Resources/SaveData/StageRecord.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            JsonData stageRecordData = JsonMapper.ToObject(jsonData);
            ParseAsStageRecordWith(stageRecordData);
        }

        // 스테이지 조건 불러오기
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
