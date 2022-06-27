using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    [SerializeField]
    int noOfLevels;
    [SerializeField]
    GameObject stageButton;
    [SerializeField]
    GameObject tutorialStageButton;

    [SerializeField]
    RectTransform parentPanel;

    [SerializeField]
    GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    SoundPlayer uISoundPlayer;

    [SerializeField]
    List<int> storyStageList = new List<int>();

    [SerializeField]
    List<int> tutorialStageList = new List<int>();

    ScrollRect scrollRect;
    RectTransform contentPanel;

    int stageReached;

    List<GameObject> btnList = new List<GameObject>();

    void Awake()
    {
        if (uISoundPlayer == null)
            throw new System.Exception("StageSelect doesnt have uISoundPlayer!");

        scrollRect = GetComponent<ScrollRect>();
        contentPanel = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

        Initialize();
        PlayBGM();
    }

    void PlayBGM()
    {
        SoundManager.Instance.BgmSoundPlayer.MakeLoop(true);
        SoundManager.Instance.BgmSoundPlayer.PlaySound(SoundManager.Instance.GetSceneBGMName(SceneManager.GetActiveScene().name));
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ForceStageOpen();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GameManager.Instance.ChangeScene("End");
        }
    }

    void Initialize()
    {
        stageReached = StageManager.Instance.StageReached;
        for (int i = 0; i < noOfLevels; i++)
        {
            GameObject stgBtn;
            if (tutorialStageList.Contains(i + 1))
                stgBtn = Instantiate(tutorialStageButton, parentPanel, false);
            else
                stgBtn = Instantiate(stageButton, parentPanel, false);
            btnList.Add(stgBtn);
            StageUIController stgUIController = stgBtn.GetComponent<StageUIController>();

            if (stgUIController != null)
            {
                stgUIController.StageNum = i + 1;
                if (storyStageList.Contains(i + 1))
                    stgUIController.HasStory = true;
                stgUIController.UISoundPlayer = uISoundPlayer;
            }
        }

        GotoCurrentStageReached();
    }

    void GotoCurrentStageReached()
    {
        Vector2 temp = gridLayoutGroup.gameObject.GetComponent<RectTransform>().anchoredPosition;
        gridLayoutGroup.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(temp.x, -btnList[stageReached].GetComponent<RectTransform>().anchoredPosition.y);
    }

    public void GoToLobbyScene()
    {
        Invoke("GotoLobbySceneDelay", 1f);
    }

    void GotoLobbySceneDelay() => GameManager.Instance.ChangeScene("Lobby");

    public void ForceStageOpen()
    {
        stageReached += 1;
        if (stageReached <= btnList.Count)
            btnList[stageReached - 1].GetComponent<StageUIController>().IsLocked = false;
    }
}
