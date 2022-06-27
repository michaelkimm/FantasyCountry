using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSoundPlayer : SoundPlayer
{
    [SerializeField]
    string soundName;

    [SerializeField]
    bool playOnAwake = true;

    protected override void Awake()
    {
        print("BGMSoundPlayer awake!");
        base.Awake();
        SoundManager.Instance.BgmSoundPlayer = this;
    }

    override protected void OnEnable()
    {
        base.OnEnable();

        if (playOnAwake)
        {
            PlaySound(soundName);
        }
    }
}
