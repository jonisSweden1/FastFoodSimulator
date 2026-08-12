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

    public void AddItemToBurger(Transform burgerItem)
    {
        if(_canStackBurger)
        {
            ItemIdentifier burgerItemId = burgerItem.GetComponent<ItemIdentifier>();

            if(burgerItemId.CurrentType != TypeOfItem.BurgerItem)
            {
                Debug.Log("This item is not a burger item");
                return;
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
            burgerItemId.HasAddedBurgerItemToStack(gameObject);

            if(burgerItemId.CurrentBurgerType == TypeOfBurgerStack.TopBun)
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