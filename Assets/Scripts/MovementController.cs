using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundPointController))]
public class MovementController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float crouchMultiplier = 0.5f;
    [SerializeField] private float jumpForce = 2400f;

    [Header("Inputs")]
    [SerializeField] private KeyCode keySprint = KeyCode.LeftShift;
    [SerializeField] private KeyCode keyCrouch = KeyCode.LeftControl;
    [SerializeField] private KeyCode keyJump = KeyCode.Space;

    [Header("Prefab Parts")]
    [SerializeField] private GroundPointController groundPoint;

    private Rigidbody rb;

    private float sprinting;
    private float crouching;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        sprinting = 1;
        crouching = 1;
    }

    private void FixedUpdate()
    {
        UpdateSprint();
        UpdateCrouch();
        UpdateMove();
    }

    private void Update()
    {
        UpdateJump();
    }

    private void UpdateSprint()
    {
        if (Input.GetKey(keySprint))
        {
            sprinting = sprintMultiplier;
        }
        else
        {
            sprinting = 1;
        }
    }

    private void UpdateCrouch()
    {
        if (Input.GetKey(keyCrouch))
        {
            crouching = crouchMultiplier;
        }
        else
        {
            crouching = 1;
        }
    }

    private void UpdateMove()
    {
        var movementVertical = Input.GetAxis("Vertical");
        var movementHorizontal = Input.GetAxis("Horizontal");

        var speedMultiplier = movementSpeed * sprinting * crouching * Time.deltaTime;

        var positionVertical = speedMultiplier * movementVertical * transform.forward;
        var positionHorizontal = speedMultiplier * movementHorizontal * transform.right;

        var positionCurrent = rb.transform.position;
        var positionNew = positionCurrent + positionVertical + positionHorizontal;

        rb.MovePosition(positionNew);
    }

    private void UpdateJump()
    {
        if (!groundPoint.Grounded)
            return;

        if (Input.GetKeyDown(keyJump))
        {
            var jumpVector = jumpForce * transform.up;
            rb.AddForce(jumpVector, ForceMode.Impulse);
        }
    }
}
