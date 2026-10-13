using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BurgerStackNormalState : BurgerStackBaseState
{
    public override void Enter(BurgerStackSystem manager)
    {

    }

    public override void Exit(BurgerStackSystem manager)
    {

    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        // Identify if the type of item is burger
        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        // Set the CurrentlyOnStacking to true before switching to another state, in order to inform the manager that the stacking process is ongoing
        // This way, the other states can check this variable when entering, and if it's true, then they will initiate a different stacking process.
        manager.CurrentlyOnStacking = true;

        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        if (burgerItemId.CurrentType != TypeOfItem.BurgerItem)
        {
            Debug.Log("This item is not a burger item");
            manager.CurrentlyOnStacking = false;
            return false;
        }

        // Check if currentStackIndex is over the minimum index limitation
        if (currentStackIndex == 0)
        {
            // If the currentStackIndex is 0, then check if the burger item is a bottom bun
            if (burgerItemId.CurrentBurgerType != TypeOfBurgerStack.BottomBun)
            {
                Debug.Log("The first burger item must be a bottom bun");
                manager.CurrentlyOnStacking = false;
                return false;
            }
        }

        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.CucumberPickle)
        {
            manager.SwitchState(manager.VegetableStackState);
            return false;
        }

        Debug.Log("Current Stack Index: " + currentStackIndex);
        Debug.Log("Previous Stack Index: " + previousStackIndex);

        Debug.Log(burgerStack[previousStackIndex]);

        // Either add burger item to the recent burger stack
        item.transform.position = burgerStack[previousStackIndex].transform.GetChild(1).position;
        Debug.Log(burgerStack[previousStackIndex].transform.GetChild(1));
        Debug.Log("Added");

        // Set parent to the burger item
        item.transform.parent = manager.transform;

        burgerStack.Add(item);
        burgerItemId.AddBurgerItemToStack(manager);

        manager.CurrentlyOnStacking = false;

        return true;
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack)
    {
        // Setting the removedItem with the toppest stack burger item
        item = burgerStack[manager.CurrentStackIndex];

        // Remove parent from the stacked burger
        item.transform.SetParent(null, true);
        item.GetComponent<ItemIdentifier>().RemoveBurgerItemFromStack();

        // Removing the burger item from the stack
        burgerStack.RemoveAt(manager.CurrentStackIndex);
        
        return true;
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {
        manager.SwitchState(manager.WrapState);
    }
}
