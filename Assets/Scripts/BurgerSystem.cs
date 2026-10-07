using System.Collections.Generic;
using UnityEngine;

public class BurgerSystem : MonoBehaviour
{
    // Field for the current stack
    private int currentStackIndex = -1;

    // List field of burger stacks that are stacked on top of each other
    private List<GameObject> _burgerStack;

    // Bool field to condition if the burger stack can be stacked or not
    private bool _canStackBurger = true;

    // A point where the burger item should be placed
    private Transform _topPoint;

    void Start()
    {
        _burgerStack = new List<GameObject>();

        _topPoint = transform;
    }

    // This is to add burger items to the burger stack.
    public bool TryAddItemToBurger(Transform burgerItem)
    {
        // Check if you can add burger item to the stack
        if (!_canStackBurger)
        {
            Debug.Log("Cannot stack burger items at this time.");
            return false;
        }

        // Identify if the type of item is burger
        ItemIdentifier burgerItemId = burgerItem.GetComponent<ItemIdentifier>();

        if (burgerItemId.CurrentType != TypeOfItem.BurgerItem)
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
            // If the currentStackIndex is -1, then check if the burger item is a bottom bun
            if (burgerItemId.CurrentBurgerType != TypeOfBurgerStack.BottomBun)
            {
                Debug.Log("The first burger item must be a bottom bun");
                return false;
            }

            // Or add burger item at the first burger item
            burgerItem.transform.position = _topPoint.position;
            Debug.Log("Added the bottom bun on the top of the wrapper paper");
        }

        // Set parent to the burger item
        burgerItem.transform.parent = transform;

        // Add burger item to the stack
        currentStackIndex++;
        _burgerStack.Add(burgerItem.gameObject);
        burgerItemId.AddBurgerItemToStack(gameObject);

        // If the burger item that is stacked is a top bun, then shut down the ability to stack the burger
        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
        {
            _canStackBurger = false;
        }

        return true;
    }

    public bool TryRemoveItemFromBurger(out GameObject removedItem)
    {
        // Check if the currentStackIndex is less than 0, which means there are no items to remove
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

    // To list all information to the player that is wrapped,
    // And not disclose the actual information of the burger stack,
    // We will return a JSON string of the burger stack items.
    public string ListAllStackedItemsInBurger()
    {
        string json = JsonUtility.ToJson(_burgerStack.ToArray());

        return json;
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