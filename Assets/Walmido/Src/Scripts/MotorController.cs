using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField]
    SwingMotor swingMotor;

    [SerializeField]
    TurnMotor turnMotor;

    void Awake()
    {
        if (swingMotor == null)
            throw new System.Exception("MotorController doesnt have swingMotor");

        if (turnMotor == null)
            throw new System.Exception("MotorController doesnt have turnMotor");
    }

    public void DeactivateMotor()
    {
        swingMotor.IsStop = true;
        turnMotor.IsStop = true;
    }

    public void ActivateMotor()
    {
        swingMotor.IsStop = false;
        turnMotor.IsStop = false;
    }
}
