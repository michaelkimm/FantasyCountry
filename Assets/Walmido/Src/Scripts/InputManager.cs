using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    PlayerRBMovementController playerMovementController;
    [SerializeField]
    FixAbility fixAbility;

    [SerializeField]
    float minMovablePercent = 0.1f;

    bool isAvailable = true;

    void Awake()
    {
        if (playerMovementController == null)
            throw new System.Exception("InputManager doesnt have playerMovementController");
        if (fixAbility == null)
            throw new System.Exception("InputManager doesnt have fixAbility");
    }

    void Update()
    {
        if (!isAvailable)
            return;

        // ���� �̵� / Ű���� ���ۿ�
        playerMovementController.MovePosition(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.F))
            Fix();
        if (Input.GetKeyUp(KeyCode.F))
            StopFixing();

        // ���� ȸ��
        if (Input.GetKey(KeyCode.J))
            playerMovementController.TurnLeft(true);
        else if (Input.GetKeyUp(KeyCode.J))
            playerMovementController.TurnLeft(false);

        // ������ ȸ��   
        if (Input.GetKey(KeyCode.L))
            playerMovementController.TurnRight(true);
        else if (Input.GetKeyUp(KeyCode.L))
            playerMovementController.TurnRight(false);

        if (Input.GetButtonDown("Jump"))
        {
            // ����
            playerMovementController.Jump();
        }
        if (Input.GetButtonDown("Dash"))
        {
            // ���
            playerMovementController.Dash();
        }
    }

    public void MakeAvailable()
    {
        isAvailable = true;
        playerMovementController.IsMovable = true;
    }

    public void MakeUnavailable()
    {
        isAvailable = false;
        playerMovementController.IsMovable = false;
    }

    public void Move(Vector2 movePose)
    {
        playerMovementController.MovePosition(movePose.x, movePose.y);
    }

    public void Rotate(Vector2 RotateVector)
    {
        if (RotateVector.magnitude < minMovablePercent)
        {
            playerMovementController.TurnLeft(false);
            playerMovementController.TurnRight(false);
        }
        else if (RotateVector.x < -minMovablePercent)
            playerMovementController.TurnLeft(true);
        else if (RotateVector.x > minMovablePercent)
            playerMovementController.TurnRight(true);
    }

    public void Fix()
    {
        fixAbility.Fix();
    }

    public void StopFixing()
    {
        fixAbility.StopFixing();
    }

    public void Jump()
    {
        // ����
        playerMovementController.Jump();
    }

    public void Dash()
    {
        // ���
        playerMovementController.Dash();
    }
}
