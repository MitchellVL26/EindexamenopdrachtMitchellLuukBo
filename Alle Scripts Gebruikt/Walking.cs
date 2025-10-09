using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    // NEW: TPS pitch settings
    [Header("TPS Pitch")]
    public float tpsPitchMin = -30f; // look down limit
    public float tpsPitchMax = 60f;  // look up limit
    private float tpsPitch = 0f;     // current TPS pitch

    private Animator animator;
    private CharacterController controller;
    private bool usingFirstPerson = false;

    private float pitch = 0f; // vertical look (FPS only)
    private Vector3 velocity; // for gravity and jumping

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SetCameraMode(false); // start in TPS

        // Lock and hide cursor for mouse look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleCameraSwitch();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (usingFirstPerson)
        {
            // Rotate player left/right
            transform.Rotate(Vector3.up * mouseX);

            // Vertical look (clamp so head doesn’t spin 360°)
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            firstPersonCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }
        else
        {
            // TPS → rotate player horizontally (yaw)
            transform.Rotate(Vector3.up * mouseX);

            // NEW: TPS vertical look (pitch)
            tpsPitch -= mouseY;
            tpsPitch = Mathf.Clamp(tpsPitch, tpsPitchMin, tpsPitchMax);

            // Apply pitch to the TPS camera only.
            // Assumes thirdPersonCamera is a child of the player or a rig aligned behind the player.
            Vector3 tpsAngles = thirdPersonCamera.transform.localEulerAngles;
            thirdPersonCamera.transform.localEulerAngles = new Vector3(tpsPitch, tpsAngles.y, 0f);
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        Vector3 inputDir = new Vector3(horizontal, 0, vertical);
        Vector3 moveDir = Vector3.zero;

        if (usingFirstPerson)
        {
            // Move relative to camera forward/right
            Vector3 camForward = firstPersonCamera.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = firstPersonCamera.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            moveDir = camForward * vertical + camRight * horizontal;
        }
        else
        {
            // TPS: move relative to player’s facing
            moveDir = transform.forward * vertical + transform.right * horizontal;
        }

        moveDir.Normalize();

        // Apply movement
        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // --- Gravity & Jumping ---
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f; // keeps player grounded

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                if (animator) animator.SetTrigger("Jump"); // optional, if you have jump anim
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Update Animator
        if (animator)
        {
            if (inputDir.magnitude > 0.1f)
            {
                animator.SetFloat("MoveX", horizontal);
                animator.SetFloat("MoveZ", vertical);
            }
            else
            {
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveZ", 0);
            }
        }
    }

    void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            usingFirstPerson = !usingFirstPerson;
            SetCameraMode(usingFirstPerson);

            // Reset pitch when switching modes
            if (usingFirstPerson)
            {
                // align FPS camera pitch to current TPS pitch to avoid snap
                pitch = tpsPitch;
                firstPersonCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            }
            else
            {
                // reset TPS pitch if you prefer (or keep it)
                // tpsPitch = 0f; // optional
                firstPersonCamera.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    void SetCameraMode(bool firstPerson)
    {
        firstPersonCamera.gameObject.SetActive(firstPerson);
        thirdPersonCamera.gameObject.SetActive(!firstPerson);
    }
}
