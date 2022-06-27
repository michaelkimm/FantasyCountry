using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class FixablePart : MonoBehaviour
{
    public enum FixState
    {
        Broken,
        Fixed,
        None
    }
    FixState state = FixState.Broken;
    public FixState State
    {
        get => state;
        set
        {
            state = value;
        }
    }

    [SerializeField]
    int[] missingPartAry = new int[(int)RidePart.RidePartType.Size];

    public bool ReturnIfFixable(int[] currentPartArt)
    {
        bool result = true;
        for (int i = 0; i < missingPartAry.Length; i++)
        {
            if (missingPartAry[i] > currentPartArt[i])
            {
                result = false;
                break;
            }
        }
        return result;
    }

    //[SerializeField]
    //Stage stage;

    [SerializeField]
    TurnMotor turnMotor;
    [SerializeField]
    SwingMotor swingMotor;
    [SerializeField]
    float fixedDelay = 5;
    public float FixedDelay { get => fixedDelay; }

    [SerializeField]
    float fixedAmountTime = 0;
    public float FixedAmountTime { get => fixedAmountTime; }

    [SerializeField]
    float decreaseRate = 2f;

    float dTurnMotorSpeedDecrease;
    float dMaxThetaDecrease;

    [SerializeField]
    bool isFixed = false;
    public bool IsFixed { get => isFixed; set { isFixed = value; } }
    void Awake()
    {
        //if (stage == null)
        //    throw new System.Exception("FixableArea doesnt have stage");

        if (turnMotor == null)
            throw new System.Exception("FixableArea doesnt have turnMotor");

        if (swingMotor == null)
            throw new System.Exception("FixableArea doesnt have swingMotor");
    }

    public UnityEvent OnFixed = new UnityEvent();

    void Update()
    {
        if (isFixed && !GameManager.Instance.isGameStopAndStoryOn && !GameManager.Instance.isGameProgramStop)
        {
            dTurnMotorSpeedDecrease = turnMotor.TurnSpeed / fixedDelay * Time.deltaTime * decreaseRate;
            dMaxThetaDecrease = swingMotor.MaxTheta / fixedDelay * Time.deltaTime * decreaseRate;
            turnMotor.TurnSpeed = turnMotor.TurnSpeed - dTurnMotorSpeedDecrease;
            swingMotor.MaxTheta = swingMotor.MaxTheta - dMaxThetaDecrease;
        }
    }

    //bool didtellStageMC = false;
    //void TellStageMCImFixed() => stage.AnnounceStageClearConditionFit();

    public void Fixed(float time)
    {
        fixedAmountTime += time;
        if (fixedAmountTime > fixedDelay && State == FixState.Broken)
        {
            // isFixed = true;
            //TellStageMCImFixed();
            OnFixed.Invoke();
            state = FixState.Fixed;
        }
    }

    public void BeingFixedStopped()
    {
        fixedAmountTime = 0;
    }

    public void Broken()
    {
        fixedAmountTime = 0;
        isFixed = false;
        state = FixState.Broken;
    }

    public void Initialized(int[] missingPartCnt_)
    {
        for (int i = 0; i < missingPartAry.Length; i++)
        {
            missingPartAry[i] = missingPartCnt_[i];
        }
    }
}
