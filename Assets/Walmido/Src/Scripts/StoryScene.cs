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
        // ������ ��û�ߴ��� �˻� �� ��ŵ ��ư ����
        if (GameManager.Instance.SeenStory1 == 1)
        {
            skipButton.gameObject.SetActive(true);
            skipButton.interactable = true;
        }

        // ���丮1 ��û ���
        if (storyNum == 1)
            GameManager.Instance.SeenStory1 = 1;
    }

    public void SkipStory()
    {
        sceneChanger.ChangeScene();
    }
}
