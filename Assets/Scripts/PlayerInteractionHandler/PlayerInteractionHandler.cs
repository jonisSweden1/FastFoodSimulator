using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script is to handle different interaction when the player use the Use action in the player input
public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private float interactDistance = 10;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private LayerMask _interactiveLayer;

    private Dictionary<string, IInteractionHandlerUseStrategy> _useStrategies;

    private InputAction _interactAction;
    private InputAction _pickUpAction;

    private void Start()
    {
        _useStrategies = new Dictionary<string, IInteractionHandlerUseStrategy>
        {
            { "Item", new ItemUseStrategy()},
            { "Storage", new StorageUseStrategy() }
        };
    }

    private void OnEnable()
    {
        Debug.Log("Enabled");

        _interactAction = playerInput.actions.FindAction("Interact");
        _pickUpAction = playerInput.actions.FindAction("PickUp");
        _interactAction.performed += _interactAction_started;
        _pickUpAction.performed += _pickUpAction_started;
    }

    private void _pickUpAction_started(InputAction.CallbackContext obj)
    {
        if (PlayerPickAndDropSystem.Instance.HasObjectHold)
        {
            PlayerPickAndDropSystem.Instance.TryPickOrDropAction(transform);
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance, _interactiveLayer))
        {
            if(hit.collider.tag == "Item")
            {
                PlayerPickAndDropSystem.Instance.TryPickOrDropAction(transform);
            }
        }
    }

    private void OnDisable()
    {
        _interactAction.performed -= _interactAction_started;
        _pickUpAction.performed -= _pickUpAction_started;
        _interactAction = null;
        _pickUpAction = null;
    }

    private void _interactAction_started(InputAction.CallbackContext obj)
    {
        Debug.Log("Interacting...");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            IInteractionHandlerUseStrategy useStrategy;
            if(_useStrategies.TryGetValue(hit.collider.tag, out useStrategy))
            {
                useStrategy.Use(hit);
            }
        }
    }
}
