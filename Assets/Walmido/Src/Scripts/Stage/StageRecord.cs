using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRecord
{
    int stageNumber;
    public int StageNumber { get => stageNumber; set { stageNumber = value; } }

    // ��� ����
    bool isLocked = true;
    public bool IsLocked { get => isLocked; set { isLocked = value; } }

    // Ŭ���� ����
    bool isCleared = false;
    public bool IsCleared { get => isCleared; set { isCleared = value; } }

    // �� ����
    int star;
    public int Star { get => star; set { star = value; } }

    public StageRecord(int stageNumber_, bool isLocked_ = true, bool isCleared_ = false, int star_ = 0)
    {
        stageNumber = stageNumber_;
        isLocked = isLocked_;
        isCleared = isCleared_;
        star = star_;
    }

    public void Clone(StageRecord target)
    {
        stageNumber = target.StageNumber;
        isLocked = target.isLocked;
        isCleared = target.isCleared;
        star = target.star;
    }

    /*
     public class Stage
{
    int stageNumber;
    public int StageNumber { get => stageNumber; set { stageNumber = value; } } 

    // ��� ����
    bool isLocked = true;
    public bool IsLocked { get => isLocked; set { isLocked = value; } }

    // Ŭ���� ����
    bool isCleared = false;
    public bool IsCleared { get => isCleared; set { isCleared = value; } }

    // �� ����
    int star;
    public int Star { get => star; set { star = value; } }
}
     */
}
