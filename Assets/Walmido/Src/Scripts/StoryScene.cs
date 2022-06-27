using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryScene : MonoBehaviour
{
    [SerializeField]
    int storyNum = 1;

    [SerializeField]
    Button skipButton;

    [SerializeField]
    SceneChanger sceneChanger;

    void Awake()
    {
        if (skipButton == null)
            throw new System.Exception("StoryScene doesnt have skipButton");

        if (sceneChanger == null)
            throw new System.Exception("StoryScene doesnt have sceneChanger");
    }

    void Start()
    {
        // 이전에 시청했는지 검사 후 스킵 버튼 설정
        if (GameManager.Instance.SeenStory1 == 1)
        {
            skipButton.gameObject.SetActive(true);
            skipButton.interactable = true;
        }

        // 스토리1 시청 기록
        if (storyNum == 1)
            GameManager.Instance.SeenStory1 = 1;
    }

    public void SkipStory()
    {
        sceneChanger.ChangeScene();
    }
}
