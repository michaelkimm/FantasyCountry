using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    [SerializeField]
    Slider bGMSlider;

    [SerializeField]
    Slider effectSoundSlider;

    void Awake()
    {
        if (bGMSlider == null)
            throw new System.Exception("GameSetting doesnt have bGMSlider");

        if (effectSoundSlider == null)
            throw new System.Exception("GameSetting doesnt have effectSoundSlider");
    }

    public void ChangeBackgroundSound(float volume)
    {
        SoundManager.Instance.ChangeAudioSound(SoundManager.SoundType.Background, volume);
    }

    public void ChangeEffectSound(float volume)
    {
        SoundManager.Instance.ChangeAudioSound(SoundManager.SoundType.Effect, volume);
    }

    public void GotoStartScene()
    {
        GameManager.Instance.ChangeScene("Lobby");
    }
    public void GameOver()
    {
        GameManager.Instance.EndGame();
    }

    public void GameStop(bool value)
    {
        GameManager.Instance.StopGame(value);
    }

    void OnEnable()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1);
        ChangeBackgroundSound(bgmVolume);
        bGMSlider.value = bgmVolume;

        float effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1);
        ChangeEffectSound(effectVolume);
        effectSoundSlider.value = effectVolume;
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat("BGMVolume", SoundManager.Instance.BackgroundVolume);
        PlayerPrefs.SetFloat("EffectVolume", SoundManager.Instance.EffectVolume);
    }
}
