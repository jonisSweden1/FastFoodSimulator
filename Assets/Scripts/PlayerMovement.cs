using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    private PlayerInput playerInput;

    [SerializeField] private float speed = 30f;

    private InputAction _moveAction;
    private InputAction _jumpAction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap?.Enable();
    }

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap?.Enable();

        _moveAction = playerInput.actions.FindAction("Move");
        _jumpAction = playerInput.actions.FindAction("Jump");

        _jumpAction.started += OnJump;
    }

    private void OnDisable()
    {
        _jumpAction.started -= OnJump;

        playerInput.currentActionMap?.Disable();
    }

    private void FixedUpdate()
    {
        // Handle continuous movement input
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * speed * Time.fixedDeltaTime);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Implement jump logic here
    }
}
