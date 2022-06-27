using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

class LitJson_data
{
    public int kill_count;
    public int hit_count;

    public LitJson_data(int k, int h)
    {
        kill_count = k;
        hit_count = h;
    }

    public void getKillCount()
    {
        Debug.Log("killcnt: " + kill_count);
    }

    public void getHitCount()
    {
        Debug.Log("hitcnt: " + hit_count);
    }
}

public class LitJsonTest : MonoBehaviour
{
    string ObjectToJson(object obj)
    {
        return JsonMapper.ToJson(obj);
    }

    T JsonToObject<T>(string jsonData)
    {
        return JsonMapper.ToObject<T>(jsonData);
    }
}
