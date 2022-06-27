using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage data", menuName = "Scriptable Object/Stage data")]
public class StageSO : ScriptableObject
{
    // 스테이지 숫자
    [SerializeField]
    int stageNum;
    public int StageNum { get => stageNum; }

    // npc 갯수
    [SerializeField]
    int nPCCount;
    public int NPCCount { get => nPCCount; }

    // 부품 종류 & 갯수
    [SerializeField]
    RidePart.RidePartType[] ridePartTypes;
    public RidePart.RidePartType[] RidePartTypes { get => ridePartTypes; }
}
