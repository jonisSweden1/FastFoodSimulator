using UnityEngine;
using UnityEngine.InputSystem;

// This script is to handle different interaction when the player use the Use action in the player input
public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private float interactDistance = 10;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private LayerMask _interactiveLayer;

    private InputAction _interactAction;
    private InputAction _pickUpAction;

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
            if(hit.collider.tag == "Item")
            {
                Transform item = hit.collider.transform.parent;
                ItemIdentifier itemId;

                // Check if the object is an item, which should have the ItemIdentifier
                if (item.TryGetComponent<ItemIdentifier>(out itemId))
                {
                    if (itemId.CurrentType == TypeOfItem.WrappedPaper)
                    {
                        item.GetComponent<BurgerInfo>().UnwrapPaper();
                    }

                    // Check if the object is a burger item
                    else if (itemId.CurrentType == TypeOfItem.BurgerItem)
                    {
                        // Check if it is stacked on the burger
                        if (itemId.IsBurgerStacked)
                        {
                            itemId.BurgerSystem.WrapPaper();
                        }
                    }
                }
            }
            else if(hit.collider.tag == "Storage")
            {
                Debug.Log("Found Storage, accessing the inventory");

                hit.collider.GetComponent<StorageInteractionSystem>().InteractStorage();
            }
        }
    }
}
