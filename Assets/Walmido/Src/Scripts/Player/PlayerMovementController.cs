using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    float move_speed = 10;

    [SerializeField]
    float rotate_coef = 60;

    [SerializeField]
    float jumpForce = 3f;

    float gravity = -9.81f;
    float yVelocity = 0f;

    CharacterController characterController;
    Vector3 movement;
    float horInput;
    float verInput;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
            throw new System.Exception("PlayerController doesnt have characterController");
    }
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        characterController.Move(move_speed * movement * Time.deltaTime);
        transform.Rotate(Vector3.up, rotate_coef * horInput * Time.deltaTime);
    }

    public void Move(Vector3 movement_)
    {
    }
}
