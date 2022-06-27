using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIEffects;

public class StageStartUIController : MonoBehaviour
{
    [SerializeField]
    UIShiny stageEffect;

    [SerializeField]
    UIShiny startEffect;

    void Awake()
    {
        if (stageEffect == null)
            throw new System.Exception("CreditMover doesnt have stageEffect");

        if (startEffect == null)
            throw new System.Exception("CreditMover doesnt have startEffect");
    }

    public float ActivateShinny(float fadeDelay)
    {
        stageEffect.Play();
        startEffect.Play();

        float delay = stageEffect.effectPlayer.duration + fadeDelay;

        Invoke("DeactivateObject", delay);
        return delay;
    }

    void DeactivateObject() => this.gameObject.SetActive(false);
}
