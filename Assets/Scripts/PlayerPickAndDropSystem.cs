using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickAndDropSystem : MonoBehaviour
{
    public bool HasObjectHold { get; private set; }

    public static PlayerPickAndDropSystem Instance { get; private set; }

    // State Machine
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

    // Can be set in the inspector, but can also be changed at runtime
    [SerializeField] private float pickDistance = 5;

    [SerializeField] private float dropDistance = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        nonItemState = new PlayerPDNonItemState();
        holdItemState = new PlayerPDHoldItemState();

        PickDistance = pickDistance;
        DropDistance = dropDistance;

        currentState = nonItemState;
    }

    public void TryPickOrDropAction(Transform headTransform)
    {
        currentState.EnterButton(this, headTransform);

        if(currentState == holdItemState)
        {
            HasObjectHold = true;
        }
        else if(currentState == nonItemState)
        {
            HasObjectHold = false;
        }
    }

    public bool TryTakeOutItem(out GameObject itemToTakeOut)
    {
        itemToTakeOut = null;
        if(currentState == holdItemState)
        {
            itemToTakeOut = ItemObject;
            ItemObject = null;
            SwitchState(nonItemState);
            return true;
        }

        return false;
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
    }

    public void SwitchState(PlayerPDBaseState state)
    {
        currentState = state;

        Debug.Log($"Switched to {state}");
    }
}
