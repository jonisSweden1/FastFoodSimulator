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
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is not assigned. Please assign it in the inspector.");
            return;
        }
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
        // Implement look logic here
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();
        yRotation += lookInput.x * mouseSensitivity * Time.fixedDeltaTime;
        xRotation -= lookInput.y * mouseSensitivity * Time.fixedDeltaTime;

        // Clamp the vertical rotation to prevent flipping
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the camera and orientation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
