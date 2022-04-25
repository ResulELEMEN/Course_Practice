// Player Controller with : 
// input manager
// CharacterController
// Mouse look

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = 13.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;

    // for tracking downward speed, 0.0 means grounded
    float velocityY = 0.0f;

    // Character Controller
    CharacterController characterController = null;

    // smoothing func variables
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked; // locks the cursor center of the screen
            Cursor.visible = false;                   // hides the cursor
        }
    }

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        // get input
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // smoothing
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        // Calculate pitch
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;  // -= inverted mouse y-axis
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        // rotate the Cam
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        // rotate the Body
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        // get input
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // normalize it for proper velocity calc
        targetDir.Normalize();

        // smoothing
        currentDir = Vector2.SmoothDamp(
            currentDir, 
            targetDir, 
            ref currentDirVelocity, moveSmoothTime
            );

        // check ground, if grounded no gravity
        if (characterController.isGrounded)
            velocityY = 0.0f;

        velocityY -= gravity * Time.deltaTime;

        // adding forward, right up vectors and multiplying by speed
        Vector3 velocity = (
            transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed 
            + 
            Vector3.up * velocityY;

        // Send value to  character controller
        characterController.Move(velocity * Time.deltaTime);

    }
}
