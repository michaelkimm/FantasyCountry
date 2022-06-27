using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUIController : MonoBehaviour
{
    [SerializeField]
    Button jumpBtn;

    [SerializeField]
    Button pushBtn;

    [SerializeField]
    Button fixBtn;

    void Awake()
    {
        if (jumpBtn == null)
            throw new System.Exception("InputUIController doesn't have jumpBtn");
        if (pushBtn == null)
            throw new System.Exception("InputUIController doesn't have pushBtn");
        if (fixBtn == null)
            throw new System.Exception("InputUIController doesn't have fixBtn");
    }

    public void ActivatePushBtn(bool value) => pushBtn.gameObject.SetActive(value);

    public void TwinkleFixBtn(bool value)
    {
        fixBtn.gameObject.SetActive(true);
        fixBtn.gameObject.GetComponent<ImageTwinkle>()?.Twinkle(value);
    }

    public void TwinklePushBtn(bool value)
    {
        pushBtn.gameObject.SetActive(true);
        pushBtn.gameObject.GetComponent<ImageTwinkle>()?.Twinkle(value);
    }

    public void TwinkleFixBtnWithTime(float delay)
    {
        fixBtn.gameObject.SetActive(true);
        fixBtn.gameObject.GetComponent<ImageTwinkle>()?.TwinkleWithTime(delay);
    }
    public void TwinklePushBtnWithTime(float delay)
    {
        pushBtn.gameObject.SetActive(true);
        pushBtn.gameObject.GetComponent<ImageTwinkle>()?.TwinkleWithTime(delay);
    }
}
