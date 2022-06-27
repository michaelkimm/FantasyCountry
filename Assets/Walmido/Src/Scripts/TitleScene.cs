using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{

    [SerializeField]
    GameObject titleSceneUI;

    [SerializeField]
    GameObject titleSceneSettingUI;

    [SerializeField]
    GameObject stageSelectUI;

    [SerializeField]
    GameObject endUI;

    void Start()
    {
        if (StageManager.Instance.NeedToShowEndUI)
        {
            ShowEndUI();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ShowEndUI();
        }
    }

    void ShowEndUI()
    {
        endUI.SetActive(true);
        endUI.GetComponent<EndUIController>()?.ShowLetterCover();
        StageManager.Instance.NeedToShowEndUI = false;
    }
}
