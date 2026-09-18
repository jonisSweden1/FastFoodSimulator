using UnityEngine;
using UnityEngine.InputSystem;

// This script is to handle different interaction when the player use the Use action in the player input
public class PlayerInteractionHandler : MonoBehaviour
{
    private float interactDistance = 10;

    [SerializeField] private PlayerInput playerInput;

    private InputAction _interactAction;

    private void OnEnable()
    {
        _interactAction = playerInput.actions.FindAction("Interact");
        _interactAction.started += _interactAction_started;
    }

    private void OnDisable()
    {
        _interactAction.started -= _interactAction_started;
        _interactAction = null;
    }

    private void _interactAction_started(InputAction.CallbackContext obj)
    {
        if(PlayerPickAndDropSystem.Instance.HasObjectHold)
        {
            PlayerPickAndDropSystem.Instance.TryPickOrDropAction(transform);
            return;
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            string dataTag = hit.collider.tag;

            switch(dataTag)
            {
                case "Item":
                    PlayerPickAndDropSystem.Instance.TryPickOrDropAction(transform);
                    break;
            }
        }
    }
}
