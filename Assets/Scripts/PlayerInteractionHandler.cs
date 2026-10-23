using UnityEngine;
using UnityEngine.InputSystem;

// This script is to handle different interaction when the player use the Use action in the player input
public class PlayerInteractionHandler : MonoBehaviour
{
    private float interactDistance = 10;

    [SerializeField] private PlayerInput playerInput;

    private InputAction _interactAction;
    private InputAction _pickUpAction;

    [SerializeField] private Material _hoverMaterial;

    Material _originalMaterial;

    GameObject _hoveredGameObject;

    private bool hasHovered = false;

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
            _hoveredGameObject.GetComponent<Renderer>().material = _originalMaterial;
            hasHovered = false;
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

            if(_hoveredGameObject != null)
            {
                _hoveredGameObject.GetComponent<Renderer>().material = _originalMaterial;
                _hoveredGameObject = null;
                hasHovered = false;
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

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            if(_hoveredGameObject != null)
            {
                if(hit.collider.gameObject != _hoveredGameObject)
                {
                    _hoveredGameObject.GetComponent<Renderer>().material = _originalMaterial;
                    _hoveredGameObject = null;
                    hasHovered = false;
                    return;
                }
            }

            if(hit.collider.tag == "Item")
            {
                Renderer renderer;

                if (!hit.collider.gameObject.TryGetComponent<Renderer>(out renderer))
                {
                    Debug.LogError("There is no renderer");
                    return;
                }

                _hoveredGameObject = hit.collider.gameObject;
                renderer = _hoveredGameObject.GetComponent<Renderer>();

                if(!hasHovered)
                {
                    _originalMaterial = renderer.material;
                    hasHovered = true;
                }

                _hoveredGameObject.GetComponent<Renderer>().material = _hoverMaterial;
            }
        }
    }

    private void _interactAction_started(InputAction.CallbackContext obj)
    {
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
        }
    }
}
