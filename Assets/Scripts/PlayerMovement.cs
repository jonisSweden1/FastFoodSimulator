using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;

    private PlayerGroundChecker _groundChecker;

    private PlayerCrouchController _crouchController;

    private PlayerInput _playerInput;

    [SerializeField][Tooltip("Place an orientation object")] private Transform orientation; 

    private float _speed = 30f;

    [SerializeField] private float m_walkSpeed = 30f;
    [SerializeField] private float m_sprintSpeed = 50f;

    [SerializeField] private float m_gravity = -9.81f;

    [SerializeField] private float m_jumpHeight = 1.5f;

    // To control the velocity between moving and jumping
    private Vector3 velocity;

    // Input actions
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;

    private void Awake()
    {
        // Get the character controller and ground checker
        _controller = GetComponent<CharacterController>();
        _groundChecker = GetComponentInChildren<PlayerGroundChecker>();
    }

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.currentActionMap?.Enable();
    }

    private void OnEnable()
    {
        // Enable the PlayerInput components action map when the script is enabled
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.currentActionMap?.Enable();

        _moveAction = _playerInput.actions.FindAction("Move");
        _jumpAction = _playerInput.actions.FindAction("Jump");
        _sprintAction = _playerInput.actions.FindAction("Sprint");

        _jumpAction.started += _jumpAction_started;
    }

    private void OnDisable()
    {
        _jumpAction.started -= _jumpAction_started;

        _playerInput.currentActionMap?.Disable();
    }

    private void _jumpAction_started(InputAction.CallbackContext obj)
    {
        if (_groundChecker.IsGrounded)
            velocity.y = Mathf.Sqrt(-2f * m_gravity * m_jumpHeight); 
    }

    private void Update()
    {
        // Handle sprinting input
        if (_sprintAction != null)
        {
            if (_sprintAction.IsPressed())
            {
                _speed = m_sprintSpeed; // Increase speed when sprinting
            }
            else
            {
                _speed = m_walkSpeed; // Reset to normal speed when not sprinting
            }
        }
    }

    private void FixedUpdate()
    {
        // Reset downward force accumulation when touching the ground
        if (_groundChecker.IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Slight downward force to keep grounded firmly
        }
        
        velocity.y += m_gravity * Time.fixedDeltaTime;
        
        _controller.Move(velocity * Time.fixedDeltaTime);

        // Handle continuous movement input
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 move = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        _controller.Move(move * _speed * Time.fixedDeltaTime);
    }
}
