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

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack, out bool stackAdded)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        // Debug.Log("currentStackIndex: " + currentStackIndex);
        // Debug.Log("previousStackIndex: " + previousStackIndex);

        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        if (burgerItemId.CurrentType != TypeOfItem.BurgerItem)
        {
            Debug.Log("This item is not a burger item");
            stackAdded = false;
            return false;
        }

        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Patty ||
            burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Tomato ||
            burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
        {
            AddAndSwitchStateToNormal(manager, item, burgerStack);
            stackAdded = false;
            return true;
        }

        Transform collectionPoint = burgerStack[currentStackIndex].transform.GetChild(0);

        if (burgerItemId.CurrentBurgerType == TypeOfBurgerStack.Sauce)
        {
            AddAndSwitchStateToSauceStack(manager, item, burgerStack);
            stackAdded = false;
            return true;
        }

        _currentAvailableVegetableStackIndex++;
        Debug.Log("Current Available Vegetable Stack Index:" + _currentAvailableVegetableStackIndex);

        //Debug.Log(burgerStack[previousStackIndex].transform);

        //Debug.Log(collectionPoint);

        Debug.Log(collectionPoint.childCount - 1);
        Debug.Log(_currentAvailableVegetableStackIndex);

        if (_currentAvailableVegetableStackIndex > collectionPoint.childCount - 1)
        {
            Debug.Log("No more available stack points for vegetables. Adding a new point");
            manager.SpawnCucumberPicklePoints(burgerStack[previousStackIndex].transform.GetChild(1));

            _currentAvailableVegetableStackIndex = 0;
            Debug.Log("Current Available Vegetable Stack Index:" + _currentAvailableVegetableStackIndex);

            currentStackIndex = manager.CurrentStackIndex;
            previousStackIndex = currentStackIndex - 1;

            collectionPoint = burgerStack[previousStackIndex].transform.GetChild(0);

            // Debug.Log("currentStackIndex: " + currentStackIndex);
            // Debug.Log("previousStackIndex: " + previousStackIndex);
            stackAdded = false;
        }
        else
        {
            stackAdded = false;
        }

        // Debug.Log("Set vegetable to the burger stack");

        item.transform.position = collectionPoint.GetChild(_currentAvailableVegetableStackIndex).position;
        item.transform.rotation = Quaternion.identity;

        item.transform.SetParent(collectionPoint.GetChild(_currentAvailableVegetableStackIndex));

        burgerItemId.AddBurgerItemToStack(manager);

        // Debug.Log("The burger stacking is completed");

        return true;
    }

    public void AddAndSwitchStateToNormal(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        ItemIdentifier burgerItemId = item.GetComponent<ItemIdentifier>();

        Debug.Log(currentStackIndex);

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

    public void AddAndSwitchStateToSauceStack(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        manager.SpawnSaucePoint(burgerStack[currentStackIndex].transform.GetChild(1));

        currentStackIndex = manager.CurrentStackIndex;
        previousStackIndex = currentStackIndex - 1;

        Transform collectionPoint = burgerStack[currentStackIndex].transform.GetChild(0);

        item.transform.position = collectionPoint.position;
        item.transform.rotation = Quaternion.identity;

        Quaternion currentRotation = item.transform.rotation;

        float yRotationHalf = manager.SauceYRotationRange / 2;

        float yRotationMin = currentRotation.y - yRotationHalf;
        float yRotationMax = currentRotation.y + yRotationHalf;

        float yRotation = Random.Range(yRotationMin, yRotationMax);
        Quaternion newRotation = new Quaternion(currentRotation.x, yRotation, currentRotation.z, currentRotation.w);

        item.transform.rotation = newRotation;


        Vector3 currentLocalScale = item.transform.localScale;

        float scaleRangeHalf = manager.SauceScaleRange / 2;

        float minScale = 0 - scaleRangeHalf;
        float maxScale = 0 + scaleRangeHalf;

        float newScaleValue = Random.Range(minScale, maxScale);

        Vector3 newScale = new Vector3(currentLocalScale.x + newScaleValue, currentLocalScale.y + newScaleValue, currentLocalScale.z + newScaleValue);

        item.transform.localScale = newScale;
        item.transform.SetParent(collectionPoint);

        manager.SwitchState(manager.SauceStackState);
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack, out bool stackRemoved)
    {
        int currentStackIndex = manager.CurrentStackIndex;
        int previousStackIndex = currentStackIndex - 1;

        Transform collectionPoint = burgerStack[currentStackIndex].transform.GetChild(0);
        Debug.Log(collectionPoint);

        item = collectionPoint.GetChild(_currentAvailableVegetableStackIndex).GetChild(0).gameObject;

        item.GetComponent<ItemIdentifier>().RemoveBurgerItemFromStack();

        item.transform.SetParent(null);

        stackRemoved = false;

        _currentAvailableVegetableStackIndex--;
        Debug.Log("Current Available Vegetable Stack Index:" + _currentAvailableVegetableStackIndex);

        if (_currentAvailableVegetableStackIndex < 0)
        {
            manager.DestroyPreviousStack();

            stackRemoved = true;

            // Debug.Log(burgerStack[previousStackIndex]);

            GameObject burgerItemPrevious = burgerStack[previousStackIndex];

            ItemIdentifier burgerItemPreId;

            if (!burgerItemPrevious.TryGetComponent<ItemIdentifier>(out burgerItemPreId))
            {
                if(burgerItemPrevious.name.Contains("Vegetable-stacking-point"))
                {
                    _currentAvailableVegetableStackIndex = collectionPoint.childCount - 1;
                    Debug.Log("Current Available Vegetable Stack Index:" + _currentAvailableVegetableStackIndex);
                    return true;
                }
            }

            _currentAvailableVegetableStackIndex = 0;

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
