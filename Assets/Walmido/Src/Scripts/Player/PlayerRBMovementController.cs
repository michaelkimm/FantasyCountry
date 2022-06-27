using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRBMovementController : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float RotCoef = 10f;
    [SerializeField]
    float jumpDist = 5;
    [SerializeField]
    float dashVelocity = 7.5f;

    Rigidbody rb;

    float horInput;
    float verInput;
    float rotInput;
    bool isGrounded_ = false;
    Transform modelTransform;

    public bool IsMovable = true;

    [SerializeField]
    float groundCheckDist = 0.2f;

    [SerializeField]
    LayerMask groundLayer;

    SoundPlayer soundPlayer;

    void OnDisable()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        modelTransform = transform.GetChild(0);

        soundPlayer = GetComponent<SoundPlayer>();
    }

    void Update()
    {
        if (Physics.CheckSphere(modelTransform.position, groundCheckDist))
            isGrounded_ = true;
        else
            isGrounded_ = false;

        //horInput = Input.GetAxis("Horizontal");
        //verInput = Input.GetAxis("Vertical");
        //if (Input.GetKey(KeyCode.J))
        //{
        //    rotInput = -RotCoef;
        //}
        //if (Input.GetKey(KeyCode.L))
        //{
        //    rotInput = RotCoef;
        //}

        //if (Input.GetButtonDown("Jump") && isGrounded_)
        //{
        //    rb.AddForce(transform.up * Mathf.Sqrt(jumpDist * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        //    soundPlayer.PlaySound("voice_fun_character_flying_cartoon_02");
        //}
        //if (Input.GetButtonDown("Dash"))
        //{
        //    rb.AddForce(transform.forward * dashVelocity, ForceMode.VelocityChange);
        //    soundPlayer.PlaySound("voice_fun_small_character_emote_angry_05");
        //}
    }

    void FixedUpdate()
    {
        if (!IsMovable)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        // rb.MovePosition(transform.position + (transform.forward * verInput + transform.right * horInput) * speed * Time.fixedDeltaTime);
        transform.Translate((Vector3.forward * verInput + Vector3.right * horInput) * speed * Time.fixedDeltaTime, Space.Self);
        transform.Rotate(new Vector3(0, rotInput * Time.deltaTime, 0));
    }

    public void TurnLeft(bool value)
    {
        if (value)
            rotInput = -RotCoef;
        else
            rotInput = 0;
    }
    public void TurnRight(bool value)
    {
        if (value)
            rotInput = RotCoef;
        else
            rotInput = 0;
    }
    public void MovePosition(float horInput_, float verInput_)
    {
        horInput = horInput_;
        verInput = verInput_;
    }
    public void Jump()
    {
        if (isGrounded_)
        {
            rb.AddForce(transform.up * Mathf.Sqrt(jumpDist * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            soundPlayer.PlaySound("SE_FL_PlayerJump");
        }
    }
    public void Dash()
    {
        rb.AddForce(transform.forward * dashVelocity, ForceMode.VelocityChange);
        soundPlayer.PlaySound("SE_FL_PlayerPush");
    }
}
