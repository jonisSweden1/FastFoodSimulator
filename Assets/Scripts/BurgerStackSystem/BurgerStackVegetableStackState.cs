using System.Collections.Generic;
using UnityEngine;

public class BurgerStackVegetableStackState : BurgerStackBaseState
{
    private int _currentAvailableVegetableStackIndex = 0;

    public override void Enter(BurgerStackSystem manager, List<GameObject> burgerStack)
    {
        
    }

    public override void Exit(BurgerStackSystem manager)
    {
        
    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        manager.CurrentlyOnStacking = true;

        Debug.Log(burgerStack[previousStackIndex].transform);

        Transform collectionPoint = burgerStack[previousStackIndex].transform.GetChild(0);

        Debug.Log(collectionPoint);

        if (_currentAvailableVegetableStackIndex > collectionPoint.childCount - 1)
        {
            Debug.Log("No more available stack points for vegetables. Adding a new point");
            manager.SpawnCucumberPicklePoints(burgerStack[previousStackIndex].transform.GetChild(1));
            manager.TryAddItemToBurger(item);
            return false;
        }

        Debug.Log("Set vegetable to the burger stack");

        item.transform.position = collectionPoint.GetChild(_currentAvailableVegetableStackIndex).position;
        item.transform.SetParent(collectionPoint.GetChild(_currentAvailableVegetableStackIndex));

        Debug.Log("The burger stacking is completed");

        _currentAvailableVegetableStackIndex++;

        manager.CurrentlyOnStacking = false;

        return true;
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        Transform collectionPoint = burgerStack[previousStackIndex].transform.GetChild(0);
        item = collectionPoint.GetChild(_currentAvailableVegetableStackIndex - 1).gameObject;
        _currentAvailableVegetableStackIndex--;

        if(_currentAvailableVegetableStackIndex < 0)
        {
            manager.DestroyPreviousStack();
        }

        return true;
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {
        manager.SwitchState(manager.WrapState);
    }
}
