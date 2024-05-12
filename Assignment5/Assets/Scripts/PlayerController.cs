using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

// public class PlayerController : NetworkBehaviour
public class PlayerController : NetworkBehaviour
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;

    public static void ResetStaticData() {
        OnAnyPlayerSpawned = null;
    }


    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpStrength = 8f; // Jump strength variable
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float gravityMultiplier = 3f; // Added gravity multiplier

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float ySpeed;
    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            enabled = false;
            return;
        }

    }
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();
        
        bool isJumpPressed = Input.GetButtonDown("Jump");
        MoveServerRPC(ySpeed, jumpStrength, moveAmount, moveInput, moveDir, isJumpPressed);

    }

    [ServerRpc]
    private void MoveServerRPC(float ySpeed, float jumpStrength, float moveAmount, Vector3 moveInput, Vector3 moveDir, bool isJumpPressed)
    {
        // if (!IsOwner) return;

        if (isGrounded)
        {
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump")) // Jump input handling
            {
                ySpeed = jumpStrength; // Apply jump force
            }
        }
        else
        {
            ySpeed += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void onDrawGizmosSeletected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}