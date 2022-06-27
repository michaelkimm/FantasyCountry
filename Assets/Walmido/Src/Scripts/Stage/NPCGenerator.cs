using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : ObjectGenerator
{
    [SerializeField]
    List<GameObject> nPCPrefabs = new List<GameObject>();

    Dictionary<NPC.Type, GameObject> nPCDict = new Dictionary<NPC.Type, GameObject>();

    int generateObjIdx;

    void Awake()
    {
        if (nPCPrefabs.Count != (int)NPC.Type.Size)
            throw new System.Exception("nPCPrefabs doesnt have enough GameObject");

        TurnListToDict();
    }
    void TurnListToDict()
    {
        foreach (GameObject obj in nPCPrefabs)
        {
            NPC.Type nPCType = obj.GetComponent<NPC>().NPCType;
            nPCDict.Add(nPCType, obj);
        }
    }

    protected override void GenerateObject()
    {
        // 랜덤 위치 업데이트
        base.GenerateObject();
        Instantiate(nPCDict[(NPC.Type)generateObjIdx], generatePose + transform.up * 0.5f, this.transform.rotation, transform.parent);
    }

    public void GenerateObject(float radius, float angle)
    {
        Vector3 pos = GetRandomPose(this.transform.position, radius, radius, angle, angle);
        GameObject obj = Instantiate(nPCDict[NPC.Type.DragonSD_03], pos, this.transform.rotation, transform.parent);
    }

    public void GenerateRadomNPC(int cnt)
    {
        List<int> nPCTypeIndexs = new List<int>();
        for (int i = 0; i < (int)NPC.Type.Size; i++)
        {
            nPCTypeIndexs.Add(i);
        }
        // 0,1,2,3,4,5,6

        while (cnt > 0)
        {
            // 겹치지 않는 랜덤 인덱스 얻기
            int tempIdx = Random.Range(0, nPCTypeIndexs.Count);
            generateObjIdx = nPCTypeIndexs[tempIdx];
            nPCTypeIndexs.RemoveAt(tempIdx);

            // NPC 소환
            GenerateObject();
            
            cnt -= 1;
        }
    }
}
