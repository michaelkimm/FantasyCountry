using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTwinkle : MonoBehaviour
{
    Image image;
    
    WaitForSeconds twinkleWait = new WaitForSeconds(0.5f);

    [SerializeField]
    bool twinkleOnAwake = true;
    bool isTwinkling = false;

    float twinkleTime = 0f;

    void Awake()
    {
        image = GetComponent<Image>();

        if (twinkleOnAwake)
            Twinkle();
    }

    private void OnDisable()
    {
        Twinkle(false);
    }

    public void Twinkle(bool value = true)
    {
        if (value)
            StartCoroutine("TwinkleWithRoutine");
        else
            isTwinkling = false;
    }

    IEnumerator TwinkleWithRoutine()
    {
        isTwinkling = true;

        while (isTwinkling)
        {
            yield return twinkleWait;
            if (image.enabled)
                image.enabled = false;
            else
                image.enabled = true;
        }
        image.enabled = true;
    }

    public void TwinkleWithTime(float delay)
    {
        StartCoroutine("TwinkleWithRoutine");
        Invoke("StopTwinkleDelay", delay);
    }

    void StopTwinkleDelay() => isTwinkling = false;
}
