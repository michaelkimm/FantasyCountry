using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    protected SoundManager.SoundType soundType = SoundManager.SoundType.Effect;
    public SoundManager.SoundType SoundType
    {
        get => soundType;
        set
        {
            // 사운드 등록 끄기
            StopSubscribeVolume();
            soundType = value;

            // 사운드 등록
            SubscribeVolume();
            ChangeSoundVolume();
        }
    }

    protected AudioSource soundSource;

    [SerializeField]
    bool forcePlay = false;

    virtual protected void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        if (soundSource == null)
            throw new System.Exception("SoundSource doesn't have AudioSource");
    }

    public void MakeLoop(bool value) => soundSource.loop = value;

    public void SetForcePlay(bool value) => forcePlay = value;
    public void PlaySound(string name)
    {
        // 실행 중일 경우
        if (soundSource.isPlaying && soundSource.clip != null)
        {
            // 같은게 실행중이면 그대로 진행
            // 같지 않은게 실행중에면 멈추기
            if (soundSource.clip.name != name)
            {
                soundSource.Stop();
            }
            else
            {
                if (!forcePlay)
                    return;
            }
        }
        soundSource.clip = SoundManager.Instance.GetAudioClip(name);

        if (soundSource.clip != null)
        {
            soundSource.Play(); 
        }
    }

    void soundSoucePlayDelay() => soundSource.Play();

    virtual protected void OnEnable()
    {
        SubscribeVolume();
        ChangeSoundVolume();
    }

    void OnDisable()
    {
        StopSubscribeVolume();
    }

    void ChangeSoundVolume()
    {
        float newVolume = 0f;
        switch (soundType)
        {
            case SoundManager.SoundType.Background:
                newVolume = SoundManager.Instance.BackgroundVolume;
                break;
            case SoundManager.SoundType.Effect:
                newVolume = SoundManager.Instance.EffectVolume;
                break;
            default:
                break;
        }
        soundSource.volume = newVolume;
    }

    void SubscribeVolume()
    {
        switch (soundType)
        {
            case SoundManager.SoundType.Background:
                SoundManager.Instance.OnBackgroundVolumeChanged.AddListener(ChangeSoundVolume);
                break;
            case SoundManager.SoundType.Effect:
                SoundManager.Instance.OnEffectVolumeChanged.AddListener(ChangeSoundVolume);
                break;
            default:
                break;
        }
    }

    void StopSubscribeVolume()
    {
        switch (soundType)
        {
            case SoundManager.SoundType.Background:
                SoundManager.Instance.OnBackgroundVolumeChanged.RemoveListener(ChangeSoundVolume);
                break;
            case SoundManager.SoundType.Effect:
                SoundManager.Instance.OnEffectVolumeChanged.RemoveListener(ChangeSoundVolume);
                break;
            default:
                break;
        }
    }
}
