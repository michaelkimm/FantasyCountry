using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIController : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }

    public void GameOver()
    {
        GameManager.Instance.EndGame();
    }
}
