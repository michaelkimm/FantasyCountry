using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ��� & UI ���带 �����Ѵ�.
// - ��� & UI ���� ����
// - ��� & UI ���� �Ҹ� ����
public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        Background,
        Effect
    }

    static SoundManager instance_;
    static public SoundManager Instance
    {
        get
        {
            if (instance_ == null)
            {
                GameObject gameObj = new GameObject("SoundManager");
                instance_ = gameObj.AddComponent<SoundManager>();
                instance_.Instantiate();
            }
            return instance_;
        }
    }

    //static public SoundManager Instance
    //{
    //    get
    //    {
    //        if (instance_ == null)
    //        {
    //            instance_ = new SoundManager();
    //            instance_.Instantiate();
    //        }
    //        return instance_;
    //    }
    //}

    Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();

    // ���� ���� / ������ ����
    float backgroundVolume = 1f;
    public float BackgroundVolume { get => backgroundVolume; }
    float effectVolume = 1f;
    public float EffectVolume { get => effectVolume; }
    public UnityEvent OnBackgroundVolumeChanged = new UnityEvent();
    public UnityEvent OnEffectVolumeChanged = new UnityEvent();

    // BGM
    Dictionary<string, string> sceneBGMDict = new Dictionary<string, string>();
    public string GetSceneBGMName(string sceneName) { return sceneBGMDict[sceneName]; }
    SoundPlayer bgmSoundPlayer;
    public SoundPlayer BgmSoundPlayer { get => bgmSoundPlayer; set { bgmSoundPlayer = value; } }

    void Awake()
    {
        // �̱��� ����
        if (instance_ != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance_ = this;
        }
        DontDestroyOnLoad(this.gameObject);

        LoadSounds();
    }

    public void Instantiate()
    {
        instance_.gameObject.AddComponent<AudioSource>();
        bgmSoundPlayer = instance_.gameObject.AddComponent<SoundPlayer>();
        bgmSoundPlayer.SoundType = SoundType.Background;
        LoadSounds();
        InitializeBGMDict();
    }

    void LoadSounds()
    {
        // ���� �ε�
        if (audioClipDict.Count > 0)
            return;

        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Sounds");
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioClipDict.Add(audioClips[i].name, audioClips[i]);
        }
    }

    void InitializeBGMDict()
    {
        SceneSoundSO sceneSoundSO = Resources.Load<SceneSoundSO>("ScriptableObject/SceneSound");
        if (sceneSoundSO == null)
            print("no!!");
        sceneBGMDict.Add("Lobby", sceneSoundSO.LobbySceneSoundName);
        sceneBGMDict.Add("StageSelect", sceneSoundSO.StageSelectSceneSoundName);
        sceneBGMDict.Add("End", sceneSoundSO.EndSceneSoundName);
        sceneBGMDict.Add("GyroswingB_stage_live", sceneSoundSO.IngameSceneSoundName);
        sceneBGMDict.Add("GyroswingB_stage_live_tutorial1", sceneSoundSO.IngameSceneSoundName);
        sceneBGMDict.Add("GyroswingB_stage_live_tutorial6", sceneSoundSO.IngameSceneSoundName);
    }

    public AudioClip GetAudioClip(string name)
    {
        if (audioClipDict.ContainsKey(name))
            return audioClipDict[name];
        else
            return null;
    }

    // ����� �ҽ� dict deprecated
    #region
    Dictionary<string, AudioSource> backgroundAudioSourceDict = new Dictionary<string, AudioSource>();
    Dictionary<string, AudioSource> effectAudioSourceDict = new Dictionary<string, AudioSource>();
    //public void AddAudioSource(string name, AudioSource source, SoundType soundType)
    //{
    //    switch (soundType)
    //    {
    //        case SoundType.Background:
    //            if (backgroundAudioSourceDict.ContainsKey(name))
    //                throw new System.Exception("backgroundAudioSourceDict already has " + name);
    //            backgroundAudioSourceDict.Add(name, source);
    //            break;
    //        case SoundType.Effect:
    //            if (effectAudioSourceDict.ContainsKey(name))
    //                throw new System.Exception("effectAudioSourceDict already has " + name);
    //            effectAudioSourceDict.Add(name, source);
    //            break;
    //        default:
    //            break;
    //    }

    //    // Ư�� ���忡 ��ϵ� ����� �ҽ��鿡�� ����� �� ����
    //}

    //public void DeleteAudioSource(string name, SoundType soundType)
    //{
    //    switch (soundType)
    //    {
    //        case SoundType.Background:
    //            if (backgroundAudioSourceDict.ContainsKey(name))
    //                backgroundAudioSourceDict.Remove(name);
    //            break;
    //        case SoundType.Effect:
    //            if (effectAudioSourceDict.ContainsKey(name))
    //                effectAudioSourceDict.Remove(name);
    //            break;
    //        default:
    //            break;
    //    }
    //}
    #endregion

    public void ChangeAudioSound(SoundType soundType, float volume)
    {
        switch (soundType)
        {
            case SoundType.Background:
                // ���� ����
                backgroundVolume = volume;
                // Ư�� ���忡 ��ϵ� ����� �ҽ��鿡�� ����� �� ����
                OnBackgroundVolumeChanged.Invoke();
                break;
            case SoundType.Effect:
                effectVolume = volume;
                OnEffectVolumeChanged.Invoke();
                break;
            default:
                break;
        }

    }
}
