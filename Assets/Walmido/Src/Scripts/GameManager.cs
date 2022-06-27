using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// 씬 전환 & 후 처리
// 게임 완전 정지
// 데이터 저장

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    // 나중에 고쳐!
    [SerializeField]
    int seenStory1 = 0;
    public int SeenStory1
    {
        get => seenStory1;
        set
        {
            seenStory1 = value;
            PlayerPrefs.SetInt("SeenStory1", seenStory1);
        }
    }
    public int savedNPCCount = 0;
    public int fixedRideCount = 0;

    public bool isGameProgramStop = false;
    public bool isGameStopAndStoryOn = false;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                _instance = obj.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        // 싱글톤 보장
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // 나중에 고쳐!
        seenStory1 = PlayerPrefs.GetInt("SeenStory1", 0);
        savedNPCCount = PlayerPrefs.GetInt("SavedNPCCount", 0);
        fixedRideCount = PlayerPrefs.GetInt("FixedNPCCount", 0);

        InitializeGameSound();
        //SoundManager.Instance.BgmSoundPlayer.PlaySound(SoundManager.Instance.GetSceneBGMName(SceneManager.GetActiveScene().name));
    }

    void OnApplicationQuit()
    {
        // 나중에 고쳐!
        PlayerPrefs.SetInt("SeenStory1", seenStory1);
        PlayerPrefs.SetInt("SavedNPCCount", savedNPCCount);
        PlayerPrefs.SetInt("SavedNPCCount", savedNPCCount);
        PlayerPrefs.SetInt("FixedNPCCount", fixedRideCount);
    }

    void InitializeGameSound()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1);
        SoundManager.Instance.ChangeAudioSound(SoundManager.SoundType.Background, bgmVolume);

        float effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1);
        SoundManager.Instance.ChangeAudioSound(SoundManager.SoundType.Effect, effectVolume);

    }

    public void ChangeScene(string sceneName, bool needLoadoing = false)
    {
        if (needLoadoing)
        {
            LoadingSceneController.LoadScene(sceneName);
        }
        else
            SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        StageManager.Instance.OnDisable();
        Application.Quit();
    }

    public void StopGame(bool isStop)
    {
        if (isStop)
        {
            Time.timeScale = 0f;
            isGameProgramStop = true;
            Physics.autoSimulation = false;
        }
        else
        {
            Time.timeScale = 1f;
            isGameProgramStop = false;
            Physics.autoSimulation = true;
        }
    }
}
