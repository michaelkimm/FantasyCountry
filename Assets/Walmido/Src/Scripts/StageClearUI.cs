using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearUI : StageEndUIController
{
    [SerializeField]
    GameObject oneStarText;
    [SerializeField]
    GameObject twoStarText;
    [SerializeField]
    GameObject threeStarText;

    [SerializeField]
    GameObject oneStar;
    [SerializeField]
    GameObject twoStar;
    [SerializeField]
    GameObject threeStar;

    void Awake()
    {
        if (oneStarText == null)
            throw new System.Exception("StageClearUI doesnt have oneStarText");
        if (twoStarText == null)
            throw new System.Exception("StageClearUI doesnt have twoStarText");
        if (threeStarText == null)
            throw new System.Exception("StageClearUI doesnt have threeStarText");

        if (oneStar == null)
            throw new System.Exception("StageClearUI doesnt have oneStar");
        if (twoStar == null)
            throw new System.Exception("StageClearUI doesnt have twoStar");
        if (threeStar == null)
            throw new System.Exception("StageClearUI doesnt have threeStar");
    }

    public void SetResult(int starCnt)
    {
        ShowText(starCnt);
        ShowStar(starCnt);
    }

    void ShowText(int starCnt)
    {
        oneStarText.SetActive(false);
        twoStarText.SetActive(false);
        threeStarText.SetActive(false);
        switch (starCnt)
        {
            case 1:
                oneStarText.SetActive(true);
                break;
            case 2:
                twoStarText.SetActive(true);
                break;
            case 3:
                threeStarText.SetActive(true);
                break;
            default:
                break;
        }
    }

    void ShowStar(int starCnt)
    {
        oneStar.SetActive(false);
        twoStar.SetActive(false);
        threeStar.SetActive(false);
        switch (starCnt)
        {
            case 1:
                oneStar.SetActive(true);
                break;
            case 2:
                twoStar.SetActive(true);
                break;
            case 3:
                threeStar.SetActive(true);
                break;
            default:
                break;
        }
    }
}
