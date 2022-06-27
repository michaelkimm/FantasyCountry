using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageObjectiveUI : MonoBehaviour
{
    [SerializeField]
    GameObject ratchetObj;
    [SerializeField]
    TextMeshProUGUI ratchetText;

    [SerializeField]
    GameObject voltObj;
    [SerializeField]
    TextMeshProUGUI voltText;

    [SerializeField]
    GameObject nutObj;
    [SerializeField]
    TextMeshProUGUI nutText;

    [SerializeField]
    GameObject npcCntObj;
    [SerializeField]
    TextMeshProUGUI npcCntText;

    [SerializeField]
    int currentCntFontSize = 46;
    int currentRatchectCnt = 0;
    int currentVoltCnt = 0;
    int currentNutCnt = 0;
    int currentNPCCnt = 0;

    [SerializeField]
    int objectiveCntFontSize = 36;
    int ratchetNeeded = 0;
    int voltNeeded = 0;
    int nutNeeded = 0;
    int npcNeeded = 0;

    void Awake()
    {
        if (ratchetText == null)
            throw new System.Exception("StageObjectiveUI doesnt have ratchetText");
        if (voltText == null)
            throw new System.Exception("StageObjectiveUI doesnt have voltText");
        if (nutText == null)
            throw new System.Exception("StageObjectiveUI doesnt have nutText");
        if (npcCntText == null)
            throw new System.Exception("StageObjectiveUI doesnt have npcCntText");
    }

    public void SetStageObjective(int ratchetCnt, int voltCnt, int nutCnt, int npcCnt)
    {
        ratchetNeeded = ratchetCnt;
        voltNeeded = voltCnt;
        nutNeeded = nutCnt;
        npcNeeded = npcCnt;

        UpdateStageObjective();
    }

    public void SetCurrentObjectiveState(int ratchetCnt, int voltCnt, int nutCnt, int npcCnt)
    {
        currentRatchectCnt = ratchetCnt;
        currentVoltCnt = voltCnt;
        currentNutCnt = nutCnt;
        currentNPCCnt = npcCnt;

        UpdateStageObjective();
    }

    public void UpdateStageObjective()
    {
        // 라켓
        if (ratchetNeeded != 0)
        {
            string color;
            if (currentRatchectCnt >= ratchetNeeded)
                color = "black";
            else
                color = "red";

            string temp = "<color=" + color + "><size=" + currentCntFontSize.ToString() + "px>" + currentRatchectCnt + "</size></color>";
                
            ratchetText.text = temp + "/" + ratchetNeeded.ToString();
        }
        else
            ratchetObj.SetActive(false);

        // 볼트
        if (voltNeeded != 0)
        {
            string color;
            if (currentVoltCnt >= voltNeeded)
                color = "black";
            else
                color = "red";
            string temp = "<color=" + color + "><size=" + currentCntFontSize.ToString() + "px>" + currentVoltCnt + "</size></color>";
            voltText.text = temp + "/" + voltNeeded.ToString();
        }
        else
            voltObj.SetActive(false);

        // 너트
        if (nutNeeded != 0)
        {
            string color;
            if (currentNutCnt >= nutNeeded)
                color = "black";
            else
                color = "red";
            string temp = "<color=" + color + "><size=" + currentCntFontSize.ToString() + "px>" + currentNutCnt + "</size></color>";
            nutText.text = temp + "/" + nutNeeded.ToString();
        }
        else
            nutObj.SetActive(false);

        // NPC
        if (npcNeeded != 0)
        {
            string color;
            if (currentNPCCnt >= npcNeeded)
                color = "black";
            else
                color = "red";
            string temp = "<color=" + color + "><size=" + currentCntFontSize.ToString() + "px>" + currentNPCCnt + "</size></color>";
            npcCntText.text = temp + "/" + npcNeeded.ToString();
        }
        else
            npcCntObj.SetActive(false);
    }
}
