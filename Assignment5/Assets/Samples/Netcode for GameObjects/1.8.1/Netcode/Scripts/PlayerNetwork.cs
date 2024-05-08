using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
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

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void onDrawGizmosSeletected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }git

    private void Update() {
        if (!IsOwner) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();

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

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);

    }

}
