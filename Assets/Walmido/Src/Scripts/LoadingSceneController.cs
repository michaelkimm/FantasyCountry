using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static string sceneName;

    [SerializeField]
    Slider loadingPercentSlider;

    [SerializeField]
    List<GameObject> tips = new List<GameObject>();

    [SerializeField]
    List<GameObject> dragons = new List<GameObject>();

    public static void LoadScene(string sceneName_)
    {
        sceneName = sceneName_;
        SceneManager.LoadScene("Loading");
    }

    void Awake()
    {
        if (loadingPercentSlider == null)
            throw new System.Exception("LoadingSceneController doesnt have loadingPercentSlider");
    }

    void Start()
    {
        //StartCoroutine(LoadSceneProcess());
        ActivateContents();
        StopBGM();

        Invoke("LoadSceneProcessDelay", 0.3f);
    }

    void ActivateContents()
    {
        // ÆÁ È°¼ºÈ­
        int idx = Random.Range(0, tips.Count);
        tips[idx].SetActive(true);

        // µå·¡°ï È°¼ºÈ­
        idx = Random.Range(0, dragons.Count);
        dragons[idx].SetActive(true);
    }

    void StopBGM()
    {
        SoundManager.Instance.BgmSoundPlayer.PlaySound("");
    }

    void LoadSceneProcessDelay() => StartCoroutine(LoadSceneProcess());

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float time = 0f;
        while (!op.isDone)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            if (op.progress < 0.9f)
            {
                loadingPercentSlider.value = op.progress;
            }
            else
            {
                time += 0.1f;
                loadingPercentSlider.value = Mathf.Lerp(0.9f, 1f, time * 0.25f);
                if (loadingPercentSlider.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
