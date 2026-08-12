using System.Collections.Generic;
using UnityEngine;

public class BurgerSystem : MonoBehaviour
{
    private int currentStackIndex = -1;

    private List<GameObject> _burgerStack;

    private bool _canStackBurger = false;

    private Transform _topPointBun;

    void Start()
    {
        _burgerStack = new List<GameObject>();

        _topPointBun = transform.GetChild(1);
    }

    public void ActivateBurgerStack()
    {
        _canStackBurger = true;
    }

    public void DeactivateBurgerStack()
    {
        _canStackBurger = false;
    }

    public bool AddItemToBurger(Transform burgerItem)
    {
        if(_canStackBurger)
        {
            ItemIdentifier burgerItemId = burgerItem.GetComponent<ItemIdentifier>();

            if(burgerItemId.CurrentType != TypeOfItem.BurgerItem)
            {
                Debug.Log("This item is not a burger item");
                return false;
            }

            if (currentStackIndex > -1)
            {
                burgerItem.position = _burgerStack[currentStackIndex].transform.GetChild(1).position;
                Debug.Log(_burgerStack[currentStackIndex].transform.GetChild(1));
                Debug.Log("Added");
            }
            else
            {
                burgerItem.transform.position = _topPointBun.position;
                Debug.Log("Added on the top of the bottom bun");
            }

            burgerItem.transform.parent = transform;

            currentStackIndex++;
            _burgerStack.Add(burgerItem.gameObject);
            burgerItemId.AddBurgerItemToStack(gameObject);

            if(burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
            {
                _canStackBurger = false;
            }

            return true;
        }
        else
        {
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

        removedItem = _burgerStack[currentStackIndex];
        _burgerStack.RemoveAt(currentStackIndex);
        currentStackIndex--;

        // If the removed item was the top bun, allow stacking again
        if (!_canStackBurger)
        {
            _canStackBurger = true;
        }

        return true;
    }

    public GameObject[] ListAllStackedItemsInBurger()
    {
        return _burgerStack.ToArray();
    }
}

public enum TypeOfBurgerStack
{
    Patty,
    Vegetables,
    Paper,
    BottomBun,
    TopBun,
    Sauce,
}