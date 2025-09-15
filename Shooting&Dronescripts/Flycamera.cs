using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float sprintMultiplier = 2f;
    public float lookSensitivity = 2f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Lock the cursor to the screen center and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= sprintMultiplier;

        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S
        float upDown = 0f;

        if (Input.GetKey(KeyCode.E)) upDown += 1f; // Move up
        if (Input.GetKey(KeyCode.Q)) upDown -= 1f; // Move down

        Vector3 direction = new Vector3(horizontal, upDown, vertical);
        Vector3 velocity = transform.TransformDirection(direction) * speed * Time.deltaTime;

        transform.position += velocity;
    }

    void HandleMouseLook()
    {
        rotationX += Input.GetAxis("Mouse X") * lookSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * lookSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }
}
