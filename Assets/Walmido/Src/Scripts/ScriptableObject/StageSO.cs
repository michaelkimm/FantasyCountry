using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage data", menuName = "Scriptable Object/Stage data")]
public class StageSO : ScriptableObject
{
    // �������� ����
    [SerializeField]
    int stageNum;
    public int StageNum { get => stageNum; }

    // npc ����
    [SerializeField]
    int nPCCount;
    public int NPCCount { get => nPCCount; }

    // ��ǰ ���� & ����
    [SerializeField]
    RidePart.RidePartType[] ridePartTypes;
    public RidePart.RidePartType[] RidePartTypes { get => ridePartTypes; }
}
