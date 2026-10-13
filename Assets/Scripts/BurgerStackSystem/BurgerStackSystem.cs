using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BurgerStackSystem : MonoBehaviour
{
    // Statemachine field to manage the state of the burger stack
    private BurgerStackBaseState _currentState;
    public BurgerStackNormalState NormalState { get; private set; }
    public BurgerStackSauceStackState SauceStackState { get; private set; }
    public BurgerStackVegetableStackState VegetableStackState { get; private set; }
    public BurgerStackWrapState WrapState { get; private set; }

    // Field for the current stack
    private int currentStackIndex = 0;

    public int CurrentStackIndex
    {
        get { return currentStackIndex; }
    }

    // List field of burger stacks that are stacked on top of each other
    private List<GameObject> _burgerStack;

    // Bool field to condition if the burger stack can be stacked or not
    private bool _canStackBurger = true;

    void Start()
    {
        _burgerStack = new List<GameObject>();
        _burgerStack.Add(gameObject);

        Debug.Log(_burgerStack[0]);

        NormalState = new BurgerStackNormalState();
        SauceStackState = new BurgerStackSauceStackState();
        VegetableStackState = new BurgerStackVegetableStackState();
        WrapState = new BurgerStackWrapState();

        _currentState = NormalState;
        _currentState.Enter(this);
    }

    private void OnDisable()
    {
        _currentState?.Exit(this);
    }

    public void WrapPaper()
    {
        _currentState.WrapPaper(this);
    }

    // This is to add burger items to the burger stack.
    public bool TryAddItemToBurger(GameObject burgerItem)
    {
        // Check if you can add burger item to the stack
        if (!_canStackBurger)
        {
            Debug.Log("Cannot stack burger items at this time.");
            return false;
        }

        // Identify if the type of item is burger
        ItemIdentifier burgerItemId = burgerItem.GetComponent<ItemIdentifier>();

        currentStackIndex++;
        if (!_currentState.AddItemToBurger(this, burgerItem, _burgerStack))
        {
            currentStackIndex--;
            return false;
        }

        // If the burger item that is stacked is a top bun, then shut down the ability to stack the burger
        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
        {
            _canStackBurger = false;
        }

        return true;
    }

    public bool TryRemoveItemFromBurger(out GameObject removedItem)
    {
        // Check if the currentStackIndex is less than 1, which means there are no items to remove
        if (currentStackIndex < 1)
        {
            removedItem = null;
            return false;
        }

        if(!_currentState.RemoveItemFromBurger(this, out removedItem, _burgerStack))
        {
            removedItem = null;
            return false;
        }

        currentStackIndex--;

        // If the removed item was the top bun, allow stacking again
        if (!_canStackBurger)
        {
            _canStackBurger = true;
        }

        return true;
    }

    // To list all information to the player that is wrapped,
    // And not disclose the actual information of the burger stack,
    // We will return a JSON string of the burger stack items.
    public string ListAllStackedItemsInBurger()
    {
        string json = JsonUtility.ToJson(_burgerStack.ToArray());

        return json;
    }

    public void SwitchState(BurgerStackBaseState state)
    {
        _currentState?.Exit(this);
        _currentState = state;
        _currentState.Enter(this);
    }
}

// Type of differnt burger items
public enum TypeOfBurgerStack
{
    Patty,
    Vegetables,
    Paper,
    BottomBun,
    TopBun,
    Sauce,
}