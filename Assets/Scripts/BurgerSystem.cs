using System.Collections.Generic;
using UnityEngine;

// TODO: Reduce dependency by having PaperActivation take responsibility of stacking burgers
public class BurgerSystem : MonoBehaviour
{
    // Field for the current stack
    private int currentStackIndex = -1;

    // List field of burger stacks
    private List<GameObject> _burgerStack;

    // Bool field to condition
    private bool _canStackBurger = false;

    // A point where the burger item should be placed
    private Transform _topPointBun;

    void Start()
    {
        _burgerStack = new List<GameObject>();

        _topPointBun = transform.GetChild(1);
    }

    // This is to activate burger stack system. This is really bad design, but works.
    // TODO: Reduce dependency by having PaperActivation take responsibility
    public void ActivateBurgerStack()
    {
        _canStackBurger = true;
    }

    // This is to activate burger stack system. This is really bad design, but works.
    // TODO: Reduce dependency by having PaperActivation take responsibility
    public void DeactivateBurgerStack()
    {
        _canStackBurger = false;
    }

    // This is to add burger items to the burger stack.
    // TODO: Rename this to TryAddItemToBurger
    public bool AddItemToBurger(Transform burgerItem)
    {
        // Check if you can add burger item to the stack
        // TODO: Simplify this if statement to 
        if(_canStackBurger)
        {
            // Identify if the type of item is burger
            ItemIdentifier burgerItemId = burgerItem.GetComponent<ItemIdentifier>();

            if(burgerItemId.CurrentType != TypeOfItem.BurgerItem)
            {
                Debug.Log("This item is not a burger item");
                return false;
            }

            // Check if currentStackIndex is over the minimum index limitation
            if (currentStackIndex > -1)
            {
                // Either add burger item to the recent burger stack
                burgerItem.position = _burgerStack[currentStackIndex].transform.GetChild(1).position;
                Debug.Log(_burgerStack[currentStackIndex].transform.GetChild(1));
                Debug.Log("Added");
            }
            else
            {
                // Or add burger item at the first burger item
                burgerItem.transform.position = _topPointBun.position;
                Debug.Log("Added on the top of the bottom bun");
            }

            // Set parent to the burger item
            burgerItem.transform.parent = transform;

            // Add burger item to the stack
            currentStackIndex++;
            _burgerStack.Add(burgerItem.gameObject);
            burgerItemId.AddBurgerItemToStack(gameObject);

            // If the burger item that is stacked is a top bun, then shut down the ability to stack the burger
            if(burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
            {
                _canStackBurger = false;
            }

            return true;
        }
        else
        {
            // If the stacking is deactivated, then there should be a notification to the player that it can't place anymore burger items
            Debug.Log("Cannot stack burger items at this time.");
            return false;
        }
    }

    public bool RemoveItemFromBurger(out GameObject removedItem)
    {
        if (currentStackIndex < 0)
        {
            removedItem = null;
            return false;
        }

        // Setting the removedItem with the toppest stack burger item
        removedItem = _burgerStack[currentStackIndex];

        // Remove parent from the stacked burger
        removedItem.transform.SetParent(null, true);

        // Removing the burger item from the stack
        _burgerStack.RemoveAt(currentStackIndex);
        currentStackIndex--;

        // If the removed item was the top bun, allow stacking again
        if (!_canStackBurger)
        {
            _canStackBurger = true;
        }

        return true;
    }

    // To list all information to the player that is wrapped
    public GameObject[] ListAllStackedItemsInBurger()
    {
        return _burgerStack.ToArray();
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