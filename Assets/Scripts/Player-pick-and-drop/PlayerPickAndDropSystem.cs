using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickAndDropSystem : MonoBehaviour
{
    public bool HasObjectHold { get; private set; } // Object hold bool property

    public static PlayerPickAndDropSystem Instance { get; private set; }

    public LayerMask LayerToDrop { get { return _layerToDrop; } }

    [SerializeField] private LayerMask _layerToDrop;

    // State Machine
    PlayerPDBaseState currentState;
    public PlayerPDNonItemState nonItemState { get; private set; }
    public PlayerPDHoldItemState holdItemState { get; private set; }

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

    public float PickDistance { get { return pickDistance; } } // Pick distance property
    public float DropDistance { get { return dropDistance; } } // Drop distance property

    private GameObject _itemObject;

    // Can be set in the inspector, but can also be changed at runtime
    // This is a distance for how long the player can pick up an item.
    [SerializeField] private float pickDistance = 5;

    // This is a distance for how long the player can drop an item.
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

        currentState = nonItemState;
    }

    // Try to pick or drop based on the current state
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

    // Try to take out the item from the player's hand
    public bool TryTakeOutItem(out GameObject itemToTakeOut)
    {
        itemToTakeOut = null;
        if(currentState == holdItemState)
        {
            itemToTakeOut = ItemObject;
            ItemObject = null;

            SwitchState(nonItemState);
            HasObjectHold = false;

            return true;
        }

        return false;
    }

    // Try to pick up the item from the inventory system
    public bool TryPickUpItem(GameObject itemToTakeUp)
    {
        if(currentState == nonItemState)
        {
            ItemObject = itemToTakeUp;
            SwitchState(holdItemState);
            HasObjectHold = true;
            return true;
        }

        return false;
    }

    // Switch state in the pick-and-drop system script
    public void SwitchState(PlayerPDBaseState state)
    {
        currentState = state;

        Debug.Log($"Switched to {state}");
    }
}
