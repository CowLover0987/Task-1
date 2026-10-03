using System.Collections;

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    public Player player;

    // MOVEMENT SETTINGS
    public float lookSensitivity = 2f;       // How fast the camera rotates
    public float gravity = -9.81f;           // Gravity force applied manually
    public float jumpHeight = 0.5f;          // How high the player can jump
    public float walkSpeed = 6f;
    public float runSpeed = 12f;

    // CAMERA REFERENCES
    public Transform cameraTransform;        // The actual Camera object
    public Transform target;                 // The player position the camera rig follows
    public float followSpeed = 5f;           // Smoothness of camera follow

    // COMPONENTS
    private CharacterController controller;  // Handles movement without physics
    private Animator animator;               // Controls animations
    private AnimatorStateInfo state;         // Tracks the current animation state

    public GameObject Settings;              // Reference to the settings menu
    public GameObject settingsDropdownButton; // Reference to the settings dropdown button

    // INPUT
    private Vector2 input;                   // Movement input (WASD or stick)
    private Vector2 lookInput;               // Look input (mouse or stick)

    // CAMERA STATE
    private float cameraPitch = 0f;          // Vertical camera rotation

    // MOVEMENT STATE
    private float verticalVelocity = 0f;     // Tracks falling speed for gravity

    void Start()
    {
        // Get required components on the player
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = animator.GetCurrentAnimatorStateInfo(0);
    }

    void Update()
    {
        // ============================================================
        // CAMERA ROTATION (HORIZONTAL AND VERTICAL)
        // ============================================================
        // Rotate the camera rig horizontally (mouse X)
        Transform camRig = cameraTransform.parent;
        camRig.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        // Rotate the camera vertically (mouse Y)
        cameraPitch -= lookInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f); // Prevent flipping
        cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);

        // ============================================================
        // MOVEMENT INPUT TO CAMERA-RELATIVE MOVEMENT
        // ============================================================

        // Raw input movement (X = left/right, Y = forward/back)
        Vector3 move = new Vector3(input.x, 0, input.y);

        // Convert movement so "forward" means "camera forward"
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Remove vertical tilt from camera so movement stays flat
        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Final camera-relative movement direction
        move = camForward * move.z + camRight * move.x;


        // ============================================================
        // ANIMATION STATE
        // ============================================================

        // Player is walking if movement vector is not tiny
        bool isWalking = move.sqrMagnitude > 0.01f;
        animator.SetBool("IsWalking", isWalking);


        float currentSpeed = walkSpeed;

        // ============================================================
        // GRAVITY
        // ============================================================

        // If grounded, reset falling speed so player sticks to ground
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // Small downward force keeps grounding stable
        }
        else
        {
            // Apply gravity over time when in the air
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Add gravity to movement vector
        // Horizontal movement
        Vector3 horizontal = new Vector3(move.x, 0, move.z) * currentSpeed;

        // Vertical movement
        Vector3 vertical = new Vector3(0, verticalVelocity, 0);

        // Combine
        Vector3 finalMove = horizontal + vertical;


        // ============================================================
        // MOVE PLAYER
        // ============================================================

        // Apply movement (horizontal * speed + vertical gravity)
        controller.Move(finalMove * Time.deltaTime);


        // ============================================================
        // ROTATE PLAYER TOWARD MOVEMENT DIRECTION
        // ============================================================

        // Ignore vertical movement when deciding rotation
        Vector3 flatMove = horizontal;
        flatMove.y = 0;


        // Rotate only if actually moving
        if (flatMove.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatMove);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }

    // Called automatically by the Input System when movement input changes
    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    // Called automatically by the Input System when look input changes
    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }



    // ============================================================
    // JUMP FUNCTION
    // ============================================================
    private void OnJump()
    {
        if (controller.isGrounded)
        {
            animator.SetTrigger("Jump");
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }


    private void OnSettings()
    {
        Time.timeScale = 0;
        Settings.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsDropdownButton);
        print("Settings opened.");
    }

    public void ExitSettings()
    {
        Time.timeScale = 1;
        Settings.SetActive(false);
        print("Settings closed.");
    }

}
