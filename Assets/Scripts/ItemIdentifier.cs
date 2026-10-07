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

    public GameObject WrapperPaper { get { return _wrapperPaper; } }
    public bool IsBurgerStacked { get { return _isBurgerStacked; } }

    // To check if the item is in the burger stack
    bool _isBurgerStacked = false;

    // Identify the bottom bun to get the burger system
    GameObject _wrapperPaper = null;

    // Name of the item field
    [SerializeField] private string _itemName;

    // The current type of item field
    [SerializeField] private TypeOfItem currentType;

    // The current type of burger type field
    [SerializeField] private TypeOfBurgerStack currentBurgerType;
    
    // Add the item to the stack
    public void AddBurgerItemToStack(GameObject wrapperPaper)
    {
        _isBurgerStacked = true;

        // Wrapper paper is the burger system, so we need to store it in the item identifier to access the burger system later.
        _wrapperPaper = wrapperPaper;
    }

    // Remove the burger item from the stack
    public void RemoveBurgerItemFromStack()
    {
        if(_wrapperPaper != null)
        {
            // Nullify the wrapper paper to avoid memory leak
            _wrapperPaper = null;
            _isBurgerStacked = false;
        }
    }
}

public enum TypeOfItem
{
    None,
    BurgerItem,
}
