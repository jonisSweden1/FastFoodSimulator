using System.Collections.Generic;
using UnityEngine;

public class BurgerSystem : MonoBehaviour
{
    private int currentStackIndex = -1;

    private List<GameObject> _burgerStack;

    private GameObject _currentStacked;
    private GameObject _previousStacked;

    private bool _canStackBurger = false;

    private Transform _topPointBun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _topPointBun = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateBurgerStack()
    {
        _canStackBurger = true;
    }

    public void AddItemToBurger(GameObject burgerItem, TypeOfBurgerStack type)
    {
        if(_canStackBurger)
        {
            if(_currentStacked != null)
            {
                Instantiate(burgerItem, _currentStacked.transform.GetChild(1).position, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(burgerItem, _topPointBun.position, Quaternion.identity, transform);
            }

            _burgerStack.Add(burgerItem);
            _currentStacked = burgerItem;

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
            if( _currentStacked != null)
            {

            }
        }
    }

    public GameObject[] ListAllStackedItemsInBurger()
    {
        return _burgerStack.ToArray();
    }

    public enum TypeOfBurgerStack
    {
        Vegetable,
        TopBun
    }
}
