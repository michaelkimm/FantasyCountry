using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageConditions
{
    // 스테이지
    int stageNum;
    public int StageNum
    {
        get => stageNum;
        set { stageNum = value; }
    }

    // 플레이 타임
    float playTime = 3f;
    public float PlayTime
    {
        get => playTime;
        set { playTime = value; }
    }

    // npc 종류 & 갯수
    //public List<NPC.NPCType> nPCTypes = new List<NPC.NPCType>();
    //public void AddNPCType(NPC.NPCType type)
    //{
    //    nPCTypes.Add(type);
    //}

    // npc 갯수
    int nPCCount;
    public int NPCCount
    {
        get => nPCCount;
        set { nPCCount = value; }
    }

    public List<RidePart.RidePartType> ridePartTypes = new List<RidePart.RidePartType>();
    public void AddRidePartType(RidePart.RidePartType type)
    {
        ridePartTypes.Add(type);
    }

    //public NPC.NPCType[] NPCTypes
    //{
    //    get
    //    {
    //        // 읽기 전용 배열로 반환
    //        NPC.NPCType[] ary = new NPC.NPCType[nPCTypes.Length];
    //        Array.AsReadOnly<NPC.NPCType>(nPCTypes).CopyTo(ary, 0);
    //        return ary;
    //    }
    //}


    //public RidePart.RidePartType[] RidePartTypes
    //{
    //    get
    //    {
    //        // 읽기 전용 배열로 반환
    //        RidePart.RidePartType[] ary = new RidePart.RidePartType[ridePartTypes.Length];
    //        Array.AsReadOnly<RidePart.RidePartType>(ridePartTypes).CopyTo(ary, 0);
    //        return ary;
    //    }
    //}

    public StageConditions(int stageNum_)
    {
        this.stageNum = stageNum_;
    }

    public void SetUp(int cnt_, RidePart.RidePartType[] ridePartTypes_)
    {
        NPCCount = cnt_;
        ridePartTypes = new List<RidePart.RidePartType>(ridePartTypes_);
    }

    //public void PrintInfo()
    //{
    //    Debug.Log("stageNum: " + stageNum);
    //    Debug.Log("playTime: " + playTime);
    //    for (int i = 0; i < nPCTypes.Count; i++)
    //    {
    //        Debug.Log((NPC.NPCType)nPCTypes[i]);
    //    }
    //    for (int i = 0; i < ridePartTypes.Count; i++)
    //    {
    //        Debug.Log((RidePart.RidePartType)ridePartTypes[i]);
    //    }
    //}
}
