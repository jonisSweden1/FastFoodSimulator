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
        _currentAvailableVegetableStackIndex = 0;
    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        Debug.Log("currentStackIndex: " + currentStackIndex);
        Debug.Log("previousStackIndex: " + previousStackIndex);

        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        if(burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Patty ||
            burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Tomato ||
            burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
        {
            // Either add burger item to the recent burger stack
            item.transform.position = burgerStack[previousStackIndex].transform.GetChild(1).position;
            //Debug.Log(burgerStack[previousStackIndex].transform.GetChild(1));
            //Debug.Log("Added");

            // Set parent to the burger item
            item.transform.parent = manager.transform;

            burgerStack.Add(item);
            burgerItemId.AddBurgerItemToStack(manager);
            manager.SwitchState(manager.NormalState);

            return true;
        }

        _currentAvailableVegetableStackIndex++;

        //Debug.Log(burgerStack[previousStackIndex].transform);

        Transform collectionPoint = burgerStack[previousStackIndex].transform.GetChild(0);

        //Debug.Log(collectionPoint);

        Debug.Log(collectionPoint.childCount - 1);
        Debug.Log(_currentAvailableVegetableStackIndex);

        if (_currentAvailableVegetableStackIndex > collectionPoint.childCount - 1)
        {
            Debug.Log("No more available stack points for vegetables. Adding a new point");
            manager.SpawnCucumberPicklePoints(burgerStack[previousStackIndex].transform.GetChild(1));

            _currentAvailableVegetableStackIndex = 0;

            currentStackIndex = manager.CurrentStackIndex;
            previousStackIndex = currentStackIndex - 1;

            collectionPoint = burgerStack[previousStackIndex].transform.GetChild(0);

            Debug.Log("currentStackIndex: " + currentStackIndex);
            Debug.Log("previousStackIndex: " + previousStackIndex);
        }

        Debug.Log("Set vegetable to the burger stack");

        item.transform.position = collectionPoint.GetChild(_currentAvailableVegetableStackIndex).position;
        item.transform.SetParent(collectionPoint.GetChild(_currentAvailableVegetableStackIndex));

        burgerItemId.AddBurgerItemToStack(manager);

        Debug.Log("The burger stacking is completed");

        return true;
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        Transform collectionPoint = burgerStack[currentStackIndex].transform.GetChild(0);
        Debug.Log(collectionPoint);

        item = collectionPoint.GetChild(_currentAvailableVegetableStackIndex).GetChild(0).gameObject;

        item.transform.SetParent(null);

        _currentAvailableVegetableStackIndex--;

        if(_currentAvailableVegetableStackIndex < 0)
        {
            manager.DestroyPreviousStack();

            currentStackIndex = manager.CurrentStackIndex;
            previousStackIndex = currentStackIndex - 1;

            Debug.Log(burgerStack[currentStackIndex]);

            GameObject burgerItemPrevious = burgerStack[currentStackIndex];

            ItemIdentifier burgerItemPreId;

            if (!burgerItemPrevious.TryGetComponent<ItemIdentifier>(out burgerItemPreId))
            {
                if(burgerItemPrevious.name.Contains("Vegetable-stacking-point"))
                {
                    _currentAvailableVegetableStackIndex = 4;
                    return true;
                }
            }

            if(burgerItemPreId.CurrentBurgerType == TypeOfBurgerStack.Patty ||
                burgerItemPreId.CurrentBurgerType == TypeOfBurgerStack.Tomato ||
                burgerItemPreId.CurrentBurgerType == TypeOfBurgerStack.BottomBun)
            {
                manager.SwitchState(manager.NormalState);
            }
        }

        return true;
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {
        manager.SwitchState(manager.WrapState);
    }
}
