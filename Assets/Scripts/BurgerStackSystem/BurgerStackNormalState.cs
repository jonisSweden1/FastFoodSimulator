using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BurgerStackNormalState : BurgerStackBaseState
{
    public override void Enter(BurgerStackSystem manager, List<GameObject> burgerStack)
    {

    }

    public override void Exit(BurgerStackSystem manager)
    {
        
    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack, out bool stackAdded)
    {
        // Identify if the type of item is burger
        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        if (burgerItemId.CurrentType != TypeOfItem.BurgerItem)
        {
            Debug.Log("This item is not a burger item");
            stackAdded = false;
            return false;
        }

        // Check if currentStackIndex is over the minimum index limitation
        if (currentStackIndex == 0)
        {
            // If the currentStackIndex is 0, then check if the burger item is a bottom bun
            if (burgerItemId.CurrentBurgerType != TypeOfBurgerStack.BottomBun)
            {
                Debug.Log("The first burger item must be a bottom bun");
                stackAdded = false;
                return false;
            }
        }

        // If the current burger item is a cucumber pickle, then switch to the vegetable stack state and return false to prevent the normal stacking process from continuing
        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.CucumberPickle)
        {
            AddAndSwitchStateToVegetableStack(manager, item, burgerStack);
            stackAdded = true;
            return true;
        }

        
        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Sauce)
        {
            AddAndSwitchStateToSauceStack(manager, item, burgerStack);
            stackAdded = true;
            return true;
        }

        // Debug.Log("Current Stack Index: " + currentStackIndex);
        // Debug.Log("Previous Stack Index: " + previousStackIndex);

        // Debug.Log(burgerStack[previousStackIndex]);

        // Either add burger item to the recent burger stack
        item.transform.position = burgerStack[previousStackIndex].transform.GetChild(1).position;
        item.transform.rotation = Quaternion.identity;

        // Debug.Log(burgerStack[previousStackIndex].transform.GetChild(1));
        // Debug.Log("Added");

        // Set parent to the burger item
        item.transform.parent = manager.transform;

        burgerStack.Add(item);
        burgerItemId.AddBurgerItemToStack(manager);

        stackAdded = true;
        return true;
    }

    public void AddAndSwitchStateToVegetableStack(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        manager.SpawnCucumberPicklePoints(burgerStack[previousStackIndex].transform.GetChild(1));

        currentStackIndex = manager.CurrentStackIndex;
        previousStackIndex = currentStackIndex - 1;

        Transform collectionPoint = burgerStack[previousStackIndex].transform.GetChild(0);

        item.transform.position = collectionPoint.GetChild(0).position;
        item.transform.rotation = Quaternion.identity;
        item.transform.SetParent(collectionPoint.GetChild(0));

        burgerItemId.AddBurgerItemToStack(manager);

        manager.SwitchState(manager.VegetableStackState);
    }

    public void AddAndSwitchStateToSauceStack(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        manager.SpawnSaucePoint(burgerStack[previousStackIndex].transform.GetChild(1));

        Transform collectionPoint = burgerStack[currentStackIndex].transform.GetChild(0);

        item.transform.position = collectionPoint.position;
        item.transform.rotation = Quaternion.identity;
        item.transform.SetParent(collectionPoint);

        manager.SwitchState(manager.SauceStackState);
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack, out bool stackRemoved)
    {
        int previousStackIndex = manager.CurrentStackIndex - 1;

        // Setting the removedItem with the toppest stack burger item
        item = burgerStack[manager.CurrentStackIndex];

        // Remove parent from the stacked burger
        item.transform.SetParent(null, true);
        item.GetComponent<ItemIdentifier>().RemoveBurgerItemFromStack();

        // Removing the burger item from the stack
        burgerStack.RemoveAt(manager.CurrentStackIndex);

        stackRemoved = true;

        if (burgerStack[previousStackIndex].name.Contains("Vegetable-stacking-point"))
        {
            manager.SwitchState(manager.VegetableStackState);
        }

        return true;
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {
        manager.SwitchState(manager.WrapState);
    }
}
