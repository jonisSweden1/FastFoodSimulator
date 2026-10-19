using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BurgerStackSystem : MonoBehaviour
{
    // State machine field to manage the state of the burger stack
    BurgerStackBaseState _currentState;

    // The previous state to roll back
    BurgerStackBaseState _previousState;

    public BurgerStackNoneState NoneState { get; private set; }
    public BurgerStackNormalState NormalState { get; private set; }
    public BurgerStackSauceStackState SauceStackState { get; private set; }
    public BurgerStackVegetableStackState VegetableStackState { get; private set; }
    public BurgerStackWrapState WrapState { get; private set; }

    // Field for the current stack
    public int currentStackIndex = 0;

    // Property to get the current stack index
    public int CurrentStackIndex
    {
        get { return currentStackIndex; }
    }

    // Prefab field for the cucumber or pickle points prefab (this is going to have a collection of defined points for the cucumber or pickles to stack)
    [SerializeField] private GameObject _cucumberPicklePointsPrefab;

    // Prefab field for the sauce point prefab (this is going to have the same collection of the same sauce prefab to spawn on the burger stack, but with different points for each sauce type)
    [SerializeField] private GameObject _saucePointPrefab;

    private GameObject _wrappedPaper;

    // List field of burger stacks that are stacked on top of each other
    private List<GameObject> _burgerStack;

    void Start()
    {
        _burgerStack = new List<GameObject>();

        // Add the wrapping paper as the first stack in the burger
        _burgerStack.Add(gameObject);

        // Initialize all states
        NoneState = new BurgerStackNoneState();
        NormalState = new BurgerStackNormalState();
        SauceStackState = new BurgerStackSauceStackState();
        VegetableStackState = new BurgerStackVegetableStackState();
        WrapState = new BurgerStackWrapState();

        // Set the state to normal stacking state
        _currentState = NormalState;

        // Call the "Enter" method whenever the 
        _currentState.Enter(this, _burgerStack);
    }

    private void OnDisable()
    {
        // Call the "Exit" whenever the script is disabled
        _currentState?.Exit(this);
    }

    private void OnEnable()
    {
        // Call the "Enter" whenever the script is enabled
        _currentState?.Enter(this, _burgerStack);
    }

    // Wrapping the paper
    public void WrapPaper()
    {
        foreach(GameObject gameObject in _burgerStack)
        {
            gameObject.SetActive(false);
        }

        _currentState.WrapPaper(this);
    }

    // This is to add burger items to the burger stack.
    public bool TryAddItemToBurger(GameObject burgerItem)
    {
        // Check if you can add burger item to the stack
        if (_currentState == NoneState)
        {
            Debug.Log("Cannot stack burger items at this time.");
            return false;
        }

        // Identify if the type of item is burger
        ItemIdentifier burgerItemId = burgerItem.GetComponent<ItemIdentifier>();

        if(burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Paper)
        {
            Debug.Log("Cannot place wrapping paper on the burger stack");
            return false;
        }

        // Debug.Log("CurrentStackIndex: " + currentStackIndex);

        currentStackIndex++;

        // Call the add item to burger. If it fails, then send error message, and return false to avoid from continuing
        if (!_currentState.AddItemToBurger(this, burgerItem, _burgerStack))
        {
            Debug.LogError("Failed to add burger item");

            // Subtract the current stack index
            // Debug.Log("Subtracting the currentStackIndex");
            currentStackIndex--;

            return false;
        }

        // If the current burger type is cucummberPickle or sauce, then subtract the current stack index
        if(burgerItemId.CurrentBurgerType == TypeOfBurgerStack.CucumberPickle || burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Sauce)
        {
            currentStackIndex--;
        }

        // If the burger item that is stacked is a top bun, then shut down the ability to stack the burger
        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
        {
            SwitchState(NoneState);
        }

        // Debug.Log("CurrentStackIndex: " + currentStackIndex);

        return true;
    }

    public bool TryRemoveItemFromBurger(out GameObject removedItem)
    {
        int previousStackIndex = currentStackIndex - 1;

        // Check if the currentStackIndex is less than 1, which means there are no items to remove
        if (currentStackIndex < 1)
        {
            removedItem = null;
            return false;
        }

        bool stackRemoved;

        // Remove the item from the burger stack
        if (!_currentState.RemoveItemFromBurger(this, out removedItem, _burgerStack, out stackRemoved))
        {
            removedItem = null;
            return false;
        }

        if (stackRemoved)
            currentStackIndex--;

        // Debug.Log("currentStackIndex: " + currentStackIndex);
        // Debug.Log("previousStackIndex: " + previousStackIndex);

        // If the removed item was the top bun, allow stacking again
        if (_currentState == NoneState)
        {
            SwitchToPreviousState();
        }

        return true;
    }

    public void SpawnCucumberPicklePoints(Transform pointToSpawn)
    {
        // Instantiate the cucumber or pickle points prefab at the position of the burger stack
        GameObject points = Instantiate(_cucumberPicklePointsPrefab, pointToSpawn.position, Quaternion.identity);
        // Set the parent of the points prefab to be the burger stack
        points.transform.SetParent(transform);

        _burgerStack.Add(points);
        currentStackIndex++;
    }

    public void SpawnSaucePoint(Transform pointToSpawn)
    {
        // Instantiate the sauce point prefab at the position of the burger stack
        GameObject points = Instantiate(_saucePointPrefab, pointToSpawn.position, Quaternion.identity);
        // Set the parent of the points prefab to be the burger stack
        points.transform.SetParent(transform);

        _burgerStack.Add(points);
        
        currentStackIndex++;
        Debug.Log("CurrentStackIndex: " + currentStackIndex);
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

        // Assign the previous state to the current state before switching to the new state
        _previousState = _currentState;

        // Assign the new state to the current state
        _currentState = state;
        _currentState.Enter(this, _burgerStack);
    }

    public void DestroyPreviousStack()
    {
        // Get reference of the object safely
        GameObject objectToDestroy = _burgerStack[currentStackIndex];

        // Remove the object from the list to prevent problem occuring
        _burgerStack.RemoveAt(currentStackIndex);

        // Destroy the object
        Destroy(objectToDestroy);

        // Subtract the current stack index
        // currentStackIndex--;
    }

    public void SwitchToPreviousState()
    {
        _currentState?.Exit(this);

        // Initialize a temporary variable to save for the previous state
        BurgerStackBaseState temp = _currentState;

        // Assign the previous state to the current state
        _currentState = _previousState;
        _previousState = temp;

        _currentState.Enter(this, _burgerStack);
    }
}

// Type of differnt burger items
public enum TypeOfBurgerStack
{
    Patty,

    // Cucumber and pickle is combined into one type to inform the manager that the player has stacked a cucumber or pickle on the burger stack,
    // and then switch state to the vegetable stack state, which will then instantiate the points prefab for the cucumber or pickle to be placed on the burger stack.
    CucumberPickle,
    Tomato,
    Paper,
    BottomBun,
    TopBun,
    Sauce,
}