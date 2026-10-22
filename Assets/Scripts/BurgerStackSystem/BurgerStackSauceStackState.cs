using System.Collections.Generic;
using UnityEngine;

public class BurgerStackSauceStackState : BurgerStackBaseState
{
    private int _currentSauceStackIndex = 0;

    public override void Enter(BurgerStackSystem manager, List<GameObject> burgerStack)
    {

    }

    public override void Exit(BurgerStackSystem manager)
    {
        
    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack, out bool stackAdded)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        if(burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Patty ||
            burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Tomato ||
            burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
        {
            AddAndSwitchStateToNormal(manager, item, burgerStack);
            stackAdded = true;
            return true;
        }

        // If the current burger item is a cucumber pickle, then switch to the vegetable stack state and return false to prevent the normal stacking process from continuing
        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.CucumberPickle)
        {
            AddAndSwitchStateToVegetableStack(manager, item, burgerStack);
            stackAdded = true;
            return true;
        }

        Transform collectionPoint = burgerStack[currentStackIndex].transform.GetChild(0);

        item.transform.position = collectionPoint.position;
        item.transform.rotation = Quaternion.identity;
        item.transform.SetParent(collectionPoint);

        _currentSauceStackIndex++;
        stackAdded = false;
        return true;
    }

    public void AddAndSwitchStateToNormal(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        // Either add burger item to the recent burger stack
        item.transform.position = burgerStack[previousStackIndex].transform.GetChild(1).position;
        item.transform.rotation = Quaternion.identity;
        //Debug.Log(burgerStack[previousStackIndex].transform.GetChild(1));
        //Debug.Log("Added");

        // Set parent to the burger item
        item.transform.parent = manager.transform;

        burgerStack.Add(item);
        burgerItemId.AddBurgerItemToStack(manager);
        manager.SwitchState(manager.NormalState);
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

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack, out bool stackRemoved)
    {
        throw new System.NotImplementedException();
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {

    }
}
