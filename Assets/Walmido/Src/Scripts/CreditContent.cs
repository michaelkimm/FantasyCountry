using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditContent : MonoBehaviour
{
    [SerializeField]
    Text savedNPCText;

    [SerializeField]
    Text fixedPartText;

    public void UpdateCreditContent()
    {
        savedNPCText.text = "도움받은 친구의 수: " + GameManager.Instance.savedNPCCount.ToString();
        fixedPartText.text = "수리한 스윙의 개수: " + GameManager.Instance.fixedRideCount.ToString();
    }
}
