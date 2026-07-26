using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickAndDropSystem : MonoBehaviour
{
    PlayerPDBaseState currentState;

    public PlayerPDNonItemState nonItemState { get; private set; }
    public PlayerPDHoldItemState holdItemState { get; private set; }

    public float PickDistance { get; private set; }

    public GameObject ItemObject { get { return _itemObject; }
        set
        {
            if(value == null)
            {
                _itemObject = null;
            }
            else
            {
                _itemObject = value;
            }
        } 
    }

    public float DropDistance { get; private set; }

    private GameObject _itemObject;

    [SerializeField] private float pickDistance = 5;

    [SerializeField] private float dropDistance = 10;

    [SerializeField] private PlayerInput playerInput;
    InputAction _interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nonItemState = new PlayerPDNonItemState();
        holdItemState = new PlayerPDHoldItemState();

        PickDistance = pickDistance;
        DropDistance = dropDistance;

        currentState = nonItemState;
        currentState.EnterState(this);
    }

    private void OnEnable()
    {
        if(playerInput != null)
        {
            _interactAction = playerInput.actions.FindAction("Interact");
            _interactAction.started += _interactAction_performed;

            Debug.Log(_interactAction);
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            _interactAction.started -= _interactAction_performed;
            _interactAction = null;
        }

        currentState.ExitState(this);
    }

    private void _interactAction_performed(InputAction.CallbackContext obj)
    {
        currentState.EnterButton(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (pickDistance.CompareTo(PickDistance) != 0)
        {
            PickDistance = pickDistance;
        }

        if (dropDistance.CompareTo(DropDistance) != 0)
        {
            DropDistance = dropDistance;
        }

        if(_interactAction.WasPressedThisFrame())
        {
            Debug.Log("Button Works");
        }

        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerPDBaseState state)
    {
        state.ExitState(this);
        currentState = state;

        Debug.Log($"Switched to {state}");

        currentState.EnterState(this);
    }
}
