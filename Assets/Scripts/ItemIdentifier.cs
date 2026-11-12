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

    public TypeOfSauce CurrentSauceType { get { return currentSauceType; } }

    public BurgerStackSystem BurgerSystem { get { return _burgerSystem; } }
    public StackingSystem StackSystem { get { return _stackSystem; } }
    public bool IsStacked { get { return _stackSystem != null; } }

    public bool isBurgerStacked { get { return _burgerSystem != null; } }

    // Identify the bottom bun to get the burger system
    BurgerStackSystem _burgerSystem = null;

    StackingSystem _stackSystem = null;

    // Name of the item field
    [SerializeField] private string _itemName;

    // The current type of item field
    [SerializeField] private TypeOfItem currentType;

    // The current type of burger field
    [SerializeField] private TypeOfBurgerStack currentBurgerType;

    // The current type of sauce field
    [SerializeField] private TypeOfSauce currentSauceType;
    
    // Add the item to the stack
    public void AddBurgerItemToStack(BurgerStackSystem burgerSystem)
    {
        // Burger system is the burger system, so we need to store it in the item identifier to access the burger system later.
        _burgerSystem = burgerSystem;
    }

    public void AddItemToStack(StackingSystem stackSystem)
    {
        _stackSystem = stackSystem;
    }

    public void RemoveItemFromStack()
    {
        if (_stackSystem != null)
        {
            // Nullify the stack system to avoid memory leak
            _stackSystem = null;
        }
    }

    // Remove the burger item from the stack
    public void RemoveBurgerItemFromStack()
    {
        if(_burgerSystem != null)
        {
            // Nullify the burger system to avoid memory leak
            _burgerSystem = null;
        }
    }
}

public enum TypeOfItem
{
    None,
    BurgerItem,
    WrappedPaper,
    SauceDispenser,
    SaucePackage, 
    BunPackage,
    CardboardBoxPackage,
    TrashBin,
    TrashBag,
    PattySpatula,
    HeaterTray
}