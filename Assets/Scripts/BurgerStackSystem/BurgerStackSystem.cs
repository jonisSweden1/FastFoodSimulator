using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BurgerStackSystem : MonoBehaviour
{
    // Statemachine field to manage the state of the burger stack
    BurgerStackBaseState _currentState;

    public BurgerStackNoneState NoneState { get; private set; }
    public BurgerStackNormalState NormalState { get; private set; }
    public BurgerStackSauceStackState SauceStackState { get; private set; }
    public BurgerStackVegetableStackState VegetableStackState { get; private set; }
    public BurgerStackWrapState WrapState { get; private set; }

    // Bool property to check if the burger stack is currently being stacked
    public bool CurrentlyOnStacking { get; set; } = false;  

    // Field for the current stack
    private int currentStackIndex = 0;

    // Property to get the current stack index
    public int CurrentStackIndex
    {
        get { return currentStackIndex; }
    }

    // Prefab field for the cucumber or pickle points prefab (this is going to have a collection of defined points for the cucumber or pickles to stack)
    [SerializeField] private GameObject _cucumberPicklePointsPrefab;

    // Prefab field for the sauce point prefab (this is going to have the same collection of the same sauce prefab to spawn on the burger stack, but with different points for each sauce type)
    [SerializeField] private GameObject _saucePointPrefab;

    // List field of burger stacks that are stacked on top of each other
    private List<GameObject> _burgerStack;

    void Start()
    {
        _burgerStack = new List<GameObject>();
        _burgerStack.Add(gameObject);

        // Debug.Log(_burgerStack[0]);

        NoneState = new BurgerStackNoneState();
        NormalState = new BurgerStackNormalState();
        SauceStackState = new BurgerStackSauceStackState();
        VegetableStackState = new BurgerStackVegetableStackState();
        WrapState = new BurgerStackWrapState();

        _currentState = NormalState;
        _currentState.Enter(this, null);
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
        if (_currentState == NoneState)
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
            SwitchState(NoneState, null);
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
        if (_currentState == NoneState)
        {
            SwitchState(NormalState, null);
        }

        return true;
    }

    public void SpawnCucumberPicklePoints(Transform pointToSpawn)
    {
        // Instantiate the cucumber or pickle points prefab at the position of the burger stack
        GameObject points = Instantiate(_cucumberPicklePointsPrefab, pointToSpawn.position, Quaternion.identity);
        // Set the parent of the points prefab to be the burger stack
        points.transform.SetParent(pointToSpawn);
    }

    // To list all information to the player that is wrapped,
    // And not disclose the actual information of the burger stack,
    // We will return a JSON string of the burger stack items.
    public string ListAllStackedItemsInBurger()
    {
        string json = JsonUtility.ToJson(_burgerStack.ToArray());

        return json;
    }

    public void SwitchState(BurgerStackBaseState state, GameObject item)
    {
        _currentState?.Exit(this);
        _currentState = state;
        _currentState.Enter(this, item);
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