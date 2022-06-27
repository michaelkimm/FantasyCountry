using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    public Vector3 forward = new Vector3(0, 0, 1);
    public AnimationCurve curve;
    public float MoveTime = 10f;
    public float speed = 0.07f;
    public float RandomFactorScale = 0;
    float rnd=0;
    Transform trans;
    Vector3 startPos;
    float curTime = 0;
    void Start()
    {
        trans = transform;
        startPos = trans.position;
        rnd = Random.Range(0, RandomFactorScale);
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        trans.localScale = Vector3.one * curve.Evaluate(curTime / (MoveTime + rnd));
        trans.Translate(forward * speed);
        if ((MoveTime+ rnd) < curTime)
        {
            trans.position = startPos;
            curTime = 0;
        }
    }
}
