using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerRBMovementController playerRBMovementController;

    [SerializeField]
    FixAbility fixAbility;

    [SerializeField]
    RideAbility rideAbility;

    [SerializeField]
    Deadable deadable;

    void Awake()
    {
        if (playerRBMovementController == null)
            throw new System.Exception("PlayerController doesn't have PlayerRBMovementController");
        if (fixAbility == null)
            throw new System.Exception("PlayerController doesn't have FixAbility");
        if (rideAbility == null)
            throw new System.Exception("PlayerController doesn't have RideAbility");
        if (deadable == null)
            throw new System.Exception("PlayerController doesn't have Deadable");
    }

    public void OnGamePlay()
    {
        playerRBMovementController.enabled = true;
        fixAbility.enabled = true;
        rideAbility.enabled = true;
        deadable.enabled = true;
    }

    public void OnGameEnd()
    {
        playerRBMovementController.enabled = false;
        fixAbility.enabled = false;
        rideAbility.enabled = false;
        deadable.enabled = false;
    }
}
