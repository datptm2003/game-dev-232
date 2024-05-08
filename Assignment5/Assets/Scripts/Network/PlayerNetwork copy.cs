using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// public class PlayerNetwork : NetworkBehaviour
public class PlayerNetworkCopy : MonoBehaviour
{
    // private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpStrength = 8f; // Jump strength variable
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float gravityMultiplier = 3f; // Added gravity multiplier
    [SerializeField] float ySpeed;

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;

    Quaternion targetRotation;
    CameraController cameraController;

    private void Awake()
    {
        // cameraController = Camera.main.GetComponent<CameraController>();
        cameraController = transform.Find("MainCamera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
