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
            // ���� ��� ����
            StopSubscribeVolume();
            soundType = value;

            // ���� ���
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
        // ���� ���� ���
        if (soundSource.isPlaying && soundSource.clip != null)
        {
            // ������ �������̸� �״�� ����
            // ���� ������ �����߿��� ���߱�
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
