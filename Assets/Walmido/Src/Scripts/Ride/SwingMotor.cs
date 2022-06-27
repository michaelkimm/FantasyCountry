using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMotor : MonoBehaviour
{
    
    // [SerializeField]
    // float swangObjLength = 1;

    [SerializeField]
    float maxTheta = 45f;
    float initialMaxTheta;
    public float InitialMaxTheta { get => initialMaxTheta; }
    public float MaxTheta
    {
        get => maxTheta;
        set
        {
            maxTheta = value;
            if (maxTheta < 0.0001f)
                maxTheta = 0;
        }
    }


    float totalTimePassed = 0f;

    [SerializeField]
    float swingSpeed = 0.5f;
    public float SwingSpeed
    {
        get => swingSpeed;
        set
        {
            swingSpeed = value;
            if (swingSpeed < 0.0001f)
                swingSpeed = 0;
        }
    }

    bool isStop = false;
    public bool IsStop { get => isStop; set { isStop = value; } }

    void Awake()
    {
        initialMaxTheta = maxTheta;
    }

    void Update()
    {
        if (isStop)
            return;

        float w0 = Mathf.Sqrt(9.81f);
        float x0 = maxTheta;
        // this.transform.Rotate(Vector3.right, w0 * w0 * Mathf.Cos(w0 * Time.deltaTime));
        this.transform.rotation = Quaternion.Euler(x0 * Mathf.Sin(w0 * totalTimePassed), 0, 0);

        totalTimePassed += Time.deltaTime * swingSpeed;
    }
}
