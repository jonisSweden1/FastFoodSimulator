using UnityEngine;

// This is a data class that will be used to identify the type of item and the type of burger stack it belongs to.
public class ItemIdentifier : MonoBehaviour
{
    // Name of the item property
    public string ItemName { get { return _itemName; } }

    // The current type of item property
    public TypeOfItem CurrentType { get { return currentType; } }

    // The current type of burger type property
    public TypeOfBurgerStack CurrentBurgerType { get { return currentBurgerType; } }

    public GameObject BottomBun { get { return _bottomBun; } }
    public bool IsBurgerStacked { get { return _isBurgerStacked; } }

    // To check if the item is in the burger stack
    bool _isBurgerStacked = false;

    // Identify the bottom bun to get the burger system
    GameObject _bottomBun = null;

    // Name of the item field
    [SerializeField] private string _itemName;

    // The current type of item field
    [SerializeField] private TypeOfItem currentType;

    // The current type of burger type field
    [SerializeField] private TypeOfBurgerStack currentBurgerType;
    
    // Add the item to the stack
    public void AddBurgerItemToStack(GameObject bottomBun)
    {
        _isBurgerStacked = true;
        _bottomBun = bottomBun;
    }

    // Remove the burger item from the stack
    public void RemoveBurgerItemFromStack()
    {
        if(_bottomBun != null)
        {
            _bottomBun = null;
            _isBurgerStacked = false;
        }
    }
}

public enum TypeOfItem
{
    None,
    BurgerItem,
}
