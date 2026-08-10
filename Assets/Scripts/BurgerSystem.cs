using System.Collections.Generic;
using UnityEngine;

public class BurgerSystem : MonoBehaviour
{
    private int currentStackIndex = -1;

    private GameObject[] _burgerStack;

    private bool _canStackBurger = false;

    private Transform _topPointBun;

    void Start()
    {
        _topPointBun = transform.GetChild(1);
    }

    public void ActivateBurgerStack()
    {
        _canStackBurger = true;
    }

    public void AddItemToBurger(GameObject burgerItem, TypeOfBurgerStack type)
    {
        if(_canStackBurger)
        {
            if(currentStackIndex > -1)
            {
                burgerItem.transform.position = _burgerStack[currentStackIndex].transform.GetChild(1).position;
                Debug.Log("Added");
            }
            else
            {
                burgerItem.transform.position = _topPointBun.position;
                Debug.Log("Added on the top of the bottom bun");
            }

            burgerItem.transform.parent = transform;

            currentStackIndex++;
            _burgerStack[currentStackIndex] = burgerItem;
            

            if(burgerItem.GetComponent<ItemIdentifier>().CurrentType == TypeOfItem.TopBun)
            {
                _canStackBurger = false;
            }
        }
    }

    public void RemoveItemFromBurger()
    {
        if (_canStackBurger)
        {
            _burgerStack[currentStackIndex] = null;
            currentStackIndex--;
        }
    }

    public GameObject[] ListAllStackedItemsInBurger()
    {
        return _burgerStack;
    }
}

public enum TypeOfBurgerStack
{
    Patty,
    Vegetables
}