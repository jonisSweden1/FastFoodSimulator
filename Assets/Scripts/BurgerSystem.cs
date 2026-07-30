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
            GameObject instance;

            if(currentStackIndex > -1)
            {
                instance = Instantiate(burgerItem, _burgerStack[currentStackIndex].transform.GetChild(1).position, Quaternion.identity, transform);
                Debug.Log("Added");
            }
            else
            {
                instance = Instantiate(burgerItem, _topPointBun.position, Quaternion.identity, transform);
                Debug.Log("Added on the top of the bottom bun");
            }

            currentStackIndex++;
            _burgerStack[currentStackIndex] = instance;
            

            if(type == TypeOfBurgerStack.TopBun)
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
    Vegetable,
    TopBun
}