using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (InventorySystem.Instance.isOpen == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // control rotation around x axis (look up and down)
            xRotation -= mouseY;

            //  we clamp the rotation so we can't Over.rotate (like)
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Control rotation around y axis (look up and down)
            yRotation += mouseX;

            // applying both rotations
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
