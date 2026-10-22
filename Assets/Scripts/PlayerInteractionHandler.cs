using UnityEngine;
using UnityEngine.InputSystem;

// This script is to handle different interaction when the player use the Use action in the player input
public class PlayerInteractionHandler : MonoBehaviour
{
    private float interactDistance = 10;

    [SerializeField] private PlayerInput playerInput;

    private InputAction _interactAction;
    private InputAction _pickUpAction;

    private void OnEnable()
    {
        _interactAction = playerInput.actions.FindAction("Interact");
        _pickUpAction = playerInput.actions.FindAction("PickUp");
        _interactAction.started += _interactAction_started;
        _pickUpAction.started += _pickUpAction_started;
    }

    private void _pickUpAction_started(InputAction.CallbackContext obj)
    {
        if (PlayerPickAndDropSystem.Instance.HasObjectHold)
        {
            PlayerPickAndDropSystem.Instance.TryPickOrDropAction(transform);
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            string dataTag = hit.collider.tag;

            switch (dataTag)
            {
                case "Item":
                    PlayerPickAndDropSystem.Instance.TryPickOrDropAction(transform);
                    break;
            }
        }
    }

    private void OnDisable()
    {
        _interactAction.started -= _interactAction_started;
        _pickUpAction.started -= _pickUpAction_started;
        _interactAction = null;
        _pickUpAction = null;
    }

    private void _interactAction_started(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            Transform item = hit.collider.transform.parent;
            ItemIdentifier itemId;

            // Check if the object is an item, which should have the ItemIdentifier
            if(item.TryGetComponent<ItemIdentifier>(out itemId))
            {
                if(itemId.CurrentType == TypeOfItem.WrappedPaper)
                {
                    item.GetComponent<BurgerInfo>().UnwrapPaper();
                }

                // Check if the object is a burger item
                else if(itemId.CurrentType == TypeOfItem.BurgerItem)
                {
                    // Check if it is stacked on the burger
                    if(itemId.IsBurgerStacked)
                    {
                        itemId.BurgerSystem.WrapPaper();
                    }
                }
            }
        }
    }
}
