using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public void SetActivateDelay(float delay)
    {
        Invoke("ActivateDelay", delay);
    }

    void ActivateDelay() => this.gameObject.SetActive(true);

    public void SetDeactivateDelay(float delay)
    {
        Invoke("DeactivateDelay", delay);
    }

    void DeactivateDelay() => this.gameObject.SetActive(false);
}
