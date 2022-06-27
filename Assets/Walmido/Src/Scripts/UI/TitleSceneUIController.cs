using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneUIController : MonoBehaviour
{
    [SerializeField]
    float playStartDelay = 1.5f;

    public void PlayGame()
    {
        Invoke("ChangeScene", playStartDelay);
    }

    void ChangeScene()
    {
        GameManager.Instance.ChangeScene("Stage Select");
    }
}
