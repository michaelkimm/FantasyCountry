using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScriptsController : MonoBehaviour
{
    [SerializeField]
    List<TutorialScript> tutorialScripts = new List<TutorialScript>();

    int currentScriptIdx = 0;
    public int CurrentScripIdx { get => currentScriptIdx; }

    public int NextScriptIdx { get => currentScriptIdx + 1; }

    public void StopShowCurrentPage(float delay)
    {
        if (currentScriptIdx < tutorialScripts.Count)
            tutorialScripts[currentScriptIdx].SetDeactivateDelay(delay);
    }

    public void ShowNextPage(float delay)
    {
        if (NextScriptIdx < tutorialScripts.Count)
        {
            tutorialScripts[NextScriptIdx].SetActivateDelay(delay);
            currentScriptIdx++;
        }
    }

    public void FlipScriptPage(float delay)
    {
        StopShowCurrentPage(delay);
        ShowNextPage(delay);
    }

    public void ShowPage(int pageNum, float delay = 0f)
    {
        currentScriptIdx = pageNum - 1;
        ShowNextPage(delay);
    }

    public void DeactivateAll()
    {
        foreach (TutorialScript script in tutorialScripts)
        {
            script.gameObject.SetActive(false);
        }
    }
}
