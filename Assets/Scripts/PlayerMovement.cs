using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    private PlayerInput playerInput;

    [SerializeField] private float speed = 5f;

    private InputAction _moveAction;
    private InputAction _jumpAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        InputSystem.actions.Disable();
        playerInput.currentActionMap?.Enable();
    }
}
