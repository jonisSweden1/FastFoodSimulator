using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickAndDropSystem : MonoBehaviour
{
    PlayerPDBaseState currentState;

    public PlayerPDNonItemState nonItemState { get; private set; }
    public PlayerPDHoldItemState holdItemState { get; private set; }

    public float PickDistance { get; private set; }

    [SerializeField] private float pickDistance = 5;

    [SerializeField] private PlayerInput playerInput;
    InputAction _interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nonItemState = new PlayerPDNonItemState();
        holdItemState = new PlayerPDHoldItemState();

        PickDistance = pickDistance;

        currentState = nonItemState;
        currentState.EnterState(this);
    }

    private void OnEnable()
    {
        if(playerInput != null)
        {
            _interactAction = playerInput.actions.FindAction("Interact");
        }

        currentState.EnterState(this);
    }

    private void OnDisable()
    {
        currentState.ExitState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (pickDistance.CompareTo(PickDistance) != 0)
        {
            PickDistance = pickDistance;
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
