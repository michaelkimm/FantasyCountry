using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    string nextSceneName = "";

    public void ChangeScene()
    {
        GameManager.Instance.ChangeScene(nextSceneName, true);
    }
}
