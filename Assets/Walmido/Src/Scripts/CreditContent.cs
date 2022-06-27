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
        savedNPCText.text = "������� ģ���� ��: " + GameManager.Instance.savedNPCCount.ToString();
        fixedPartText.text = "������ ������ ����: " + GameManager.Instance.fixedRideCount.ToString();
    }
}
