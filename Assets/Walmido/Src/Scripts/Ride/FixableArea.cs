using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixableArea : MonoBehaviour
{
    [SerializeField]
    TurnMotor turnMotor;
    [SerializeField]
    SwingMotor swingMotor;
    [SerializeField]
    int fixDelay = 5;

    float dTurnMotorSpeedDecrease;
    float dMaxThetaDecrease;

    bool isFixed = false;
    public bool IsFixed
    {
        get => isFixed;
        set
        {
            isFixed = value;
            if (isFixed)
            {
                StopMotor(fixDelay);
            }
        }
    }
    void Awake()
    {
        if (turnMotor == null)
            throw new System.Exception("FixableArea doesnt have turnMotor");

        if (swingMotor == null)
            throw new System.Exception("FixableArea doesnt have swingMotor");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            IsFixed = true;
        }
    }

    void StopMotor(int fixDelay)
    {
        dTurnMotorSpeedDecrease = turnMotor.TurnSpeed / fixDelay;
        dMaxThetaDecrease = swingMotor.MaxTheta / fixDelay;
        
        StartCoroutine("FixWithDelay", fixDelay);
    }
    IEnumerator FixWithDelay(float fixDelay)
    {
        print("FixWithDelay!");
        for (int i = 0; i < fixDelay; i++)
        {
            yield return new WaitForSeconds(1f);
            turnMotor.TurnSpeed = turnMotor.TurnSpeed - dTurnMotorSpeedDecrease;
            swingMotor.MaxTheta = swingMotor.MaxTheta - dMaxThetaDecrease;
        }
    }
}
