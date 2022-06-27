using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMotor : MonoBehaviour
{
    [SerializeField]
    float turnSpeed = 45f;
    float initialTurnSpeed;
    public float InitialTurnSpeed { get => initialTurnSpeed; }
    public float TurnSpeed
    {
        get => turnSpeed;
        set
        {
            turnSpeed = value;
            if (turnSpeed < 0.0001f)
            {
                turnSpeed = 0;

            }
        }
    }

    bool isStop = false;
    public bool IsStop { get => isStop; set { isStop = value; } }

    void Awake()
    {
        initialTurnSpeed = turnSpeed;
    }

    void Update()
    {
        if (isStop)
            return;
        this.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
    }
}
