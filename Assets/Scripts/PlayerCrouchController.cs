using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchController : MonoBehaviour
{
    [SerializeField] private float crouchHeight = 1.0f; // The height of the player when crouching
    [SerializeField] private float standingHeight = 2.0f; // The height of the player when standing

    [SerializeField] private float standingHeightCameraOffset = 1.0f; // The camera offset when standing    
    [SerializeField] private float crouchHeightCameraOffset = 0.5f; // The camera offset when crouching

    public bool IsCrouching { get; private set; } = false;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController characterController;

    private InputAction _crouchAction;

    private float timer = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(playerInput == null)
        {
            Debug.LogError("PlayerInput component is not assigned. Please assign it in the inspector.");
            return;
        }

        _crouchAction = playerInput.actions.FindAction("Crouch");
    }

    // Update is called once per frame
    void Update()
    {
        if (_crouchAction == null)
        {
            Debug.LogError("Crouch action is not assigned. Please check the PlayerInput actions.");
            return;
        }

        if (_crouchAction.triggered)
        {
            ToggleCrouch();
        }

        if(timer < 1f)
        {
            timer += Time.deltaTime * 10f;
        }

        if(IsCrouching)
        {
            transform.localPosition = Vector3.Lerp(new Vector3(0, standingHeightCameraOffset, 0), new Vector3(0, crouchHeightCameraOffset, 0), timer);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(new Vector3(0, crouchHeightCameraOffset, 0), new Vector3(0, standingHeightCameraOffset, 0), timer);
        }
    }

    private void ToggleCrouch()
    {
        if (IsCrouching)
        {
            // Stand up
            characterController.height = standingHeight;
            IsCrouching = false;
        }
        else
        {
            // Crouch down
            characterController.height = crouchHeight;
            IsCrouching = true;
        }

        timer = 0f;
    }
}
