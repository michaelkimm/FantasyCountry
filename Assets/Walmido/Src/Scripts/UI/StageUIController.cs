using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI stageNameTMPPro;

    [SerializeField]
    List<GameObject> starObjectList = new List<GameObject>();

    [SerializeField]
    Button startBtn;

    [SerializeField]
    GameObject unlockedObj;

    [SerializeField]
    GameObject lockedObj;

    SoundPlayer uISoundPlayer;
    public SoundPlayer UISoundPlayer { get => uISoundPlayer; set { uISoundPlayer = value; } }

    [SerializeField]
    float stageChangeDelay = 0.5f;

    [SerializeField]
    int stageNum = 1;
    public int StageNum { get => stageNum; set { stageNum = value; } }

    [SerializeField]
    string normalSceneName = "GyroswingB_stage_live";

    [SerializeField]
    string storySceneName = "GyroswingB_stage_live_story";

    [SerializeField]
    string tutorialScenName = "GyroswingB_stage_live_tutorial";

    string sceneName;
    bool hasStory = false;
    public bool HasStory
    {
        get => hasStory;
        set
        {
            hasStory = value;
        }
    }

    // ��� ����
    bool isLocked = true;
    public bool IsLocked
    {
        get => isLocked;
        set
        {
            isLocked = value;
            UpdateUI();
        }
    }

    // Ŭ���� ����
    bool isCleared = false;
    public bool IsCleared
    {
        get => isCleared;
        set
        {
            isCleared = value;
            UpdateUI();
        }
    }

    [SerializeField]
    int maxStar = 3;

    // �� ����
    int star = 0;
    public int Star
    {
        get { return star; }
        set
        {
            if (value > maxStar)
                star = maxStar;
            else if (value < 0)
                star = 0;
            else
                star = value;

            UpdateUI();
        }
    }

    void Awake()
    {
        //if (stageStarTMPro == null)
        //    throw new System.Exception("Assign stageStarTMPro to StageUIController!");

        if (startBtn == null)
            throw new System.Exception("Assign startBtn to StageUIController!");

        if (lockedObj == null)
            throw new System.Exception("Assign lockedObj to StageUIController!");
    }

    void Start()
    {
        // �������� �����ڿ��Լ� �������� ����� �޾ƿͼ� ����.
        StageRecord stageRecord = StageManager.Instance.GetStageRecord(stageNum);
        IsCleared = stageRecord.IsCleared;
        IsLocked = stageRecord.IsLocked;
        Star = stageRecord.Star;
        stageNameTMPPro.text = stageNum.ToString();
    }

    public void SelectStage()
    {
        if (HasStory)
            sceneName = storySceneName + stageNum.ToString();
        else
            sceneName = normalSceneName;

        // delay �� �� �ٲٱ�, ���� �������� ����
        SceneActivate();
    }

    public void SelectTutorialStage()
    {
        if (HasStory)
            sceneName = storySceneName + stageNum.ToString();
        else
            sceneName = tutorialScenName + stageNum.ToString();

        // delay �� �� �ٲٱ�, ���� �������� ����
        SceneActivate();
    }

    // delay �� �� �ٲٱ�, ���� �������� ����
    void SceneActivate()
    {
        Invoke("ChangeScene", stageChangeDelay);
        StageManager.Instance.CurrentStage = stageNum;
    }

    void ChangeScene()
    {
        GameManager.Instance.ChangeScene(sceneName, true);
    }

    void UpdateUI()
    {
        OnStageValueChanged(isLocked, isCleared, star);
    }

    public void PlayClickSound()
    {
        if (uISoundPlayer != null)
            uISoundPlayer.PlaySound("SE_FL_GameStart Cliked");
    }

    public void OnStageValueChanged(bool isLocked, bool isCleared, int star)
    {
        // �Ű������� UI ������Ʈ
        if (isLocked)
        {
            unlockedObj.gameObject.SetActive(false);
            startBtn.interactable = false;
            lockedObj.SetActive(true);
        }
        else
        {
            unlockedObj.gameObject.SetActive(true);
            startBtn.interactable = true;
            lockedObj.SetActive(false);

            foreach (GameObject obj in starObjectList)
            {
                obj.SetActive(false);
            }

            switch (star)
            {
                case 1:
                    starObjectList[0].SetActive(true);
                    break;
                case 2:
                    starObjectList[0].SetActive(true);
                    starObjectList[1].SetActive(true);
                    break;
                case 3:
                    starObjectList[0].SetActive(true);
                    starObjectList[1].SetActive(true);
                    starObjectList[2].SetActive(true);
                    break;
                default:
                    break;
            }
        }

        // stageStarTMPro.text = star + " / 3";
    }
}
