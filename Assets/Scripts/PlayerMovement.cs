using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    private PlayerGroundChecker groundChecker;

    private PlayerCrouchController crouchController;

    private PlayerInput playerInput;

    [SerializeField][Tooltip("Place an orientation object")] private Transform orientation; 

    private float speed = 30f;

    [SerializeField] private float walkSpeed = 30f;
    [SerializeField] private float sprintSpeed = 50f;

    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private float jumpHeight = 1.5f;

    // To control the velocity between moving and jumping
    private Vector3 velocity;

    // Input actions
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;

    private void Awake()
    {
        // Get the character controller and ground checker
        controller = GetComponent<CharacterController>();
        groundChecker = GetComponentInChildren<PlayerGroundChecker>();
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap?.Enable();
    }

    private void OnEnable()
    {
        // Enable the PlayerInput components action map when the script is enabled
        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap?.Enable();

        _moveAction = playerInput.actions.FindAction("Move");
        _jumpAction = playerInput.actions.FindAction("Jump");
        _sprintAction = playerInput.actions.FindAction("Sprint");

        _jumpAction.started += _jumpAction_started;
    }

    private void OnDisable()
    {
        _jumpAction.started -= _jumpAction_started;

        playerInput.currentActionMap?.Disable();
    }

    private void _jumpAction_started(InputAction.CallbackContext obj)
    {
        if (groundChecker.IsGrounded)
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight); 
    }

    private void Update()
    {
        // Handle sprinting input
        if (_sprintAction != null)
        {
            if (_sprintAction.IsPressed())
            {
                speed = sprintSpeed; // Increase speed when sprinting
            }
            else
            {
                speed = walkSpeed; // Reset to normal speed when not sprinting
            }
        }
    }

    private void FixedUpdate()
    {
        // Reset downward force accumulation when touching the ground
        if (groundChecker.IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Slight downward force to keep grounded firmly
        }
        
        velocity.y += gravity * Time.fixedDeltaTime;
        
        controller.Move(velocity * Time.fixedDeltaTime);

        // Handle continuous movement input
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 move = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        controller.Move(move * speed * Time.fixedDeltaTime);
    }
}
