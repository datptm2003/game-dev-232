using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5;
    [SerializeField] float x = 0;
    [SerializeField] float y = 0;
    [SerializeField] float z = 5;
    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationX;
    float rotationY;

    float invertXVal;
    float invertYVal;

    private void Awake()
    {
        followTarget = transform.parent.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (InventorySystem.Instance.isOpen == false &&
            CraftingController.Instance.isOpen == false &&
            Market.Instance.isOpen == false &&
            QuestController.Instance.isOpen == false &&
            NPC.Instance.isOpen == false
        )
        {
            invertXVal = (invertX) ? -1 : 1;
            invertYVal = (invertY) ? -1 : 1;

            rotationX += Input.GetAxis("Camera Y") * invertYVal * rotationSpeed;
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

            rotationY -= Input.GetAxis("Camera X") * invertXVal * rotationSpeed;

            var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

            var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

            transform.position = focusPosition - targetRotation * new Vector3(x, y, z);
            transform.rotation = targetRotation;
        }
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}
