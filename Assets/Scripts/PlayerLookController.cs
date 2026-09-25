using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLookController : MonoBehaviour
{
    [SerializeField] private Transform orientation;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float mouseSensitivity = 100f;

    private InputAction _lookAction;

    private float xRotation;
    private float yRotation;

    void Awake()
    {
        // If the input script is not assigned, send message and skip
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is not assigned. Please assign it in the inspector.");
            return;
        }

        // Find an action of look
        _lookAction = playerInput.actions.FindAction("Look");
    }

    private void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Look();
    }

    private void Look()
    {
        // Read the value of look input in the player input
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();

        // Divide and convert the rotations by the input of look
        yRotation += lookInput.x * mouseSensitivity * Time.fixedDeltaTime;
        xRotation -= lookInput.y * mouseSensitivity * Time.fixedDeltaTime;

        // Clamp the vertical rotation to prevent flipping
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the camera and the orientation of the player
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
