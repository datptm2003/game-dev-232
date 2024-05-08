using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork2 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public float gravity = -9.81f * 2;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (!IsOwner) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;

        transform.position += velocity * moveSpeed * Time.deltaTime;
    }
}
