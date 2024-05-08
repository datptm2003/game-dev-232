using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// public class PlayerNetwork : NetworkBehaviour
public class PlayerNetwork : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpStrength = 8f; // Jump strength variable
    public float gravity = -9.81f * 2;


    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    Vector3 velocity;

    void Start()
    {
        // controller = GetComponent<CharacterController>();
    }

    void update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // right is the red Axis, forward is the blue axis
        Vector3 moveDir = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        transform.position = moveDir * moveSpeed * Time.deltaTime;

        // velocity.y += gravity * Time.deltaTime;

        // transform.position = velocity * Time.deltaTime;
    }
}
