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
    public TrayStackSystem TraySystem { get { return _traySystem; } }
    public bool IsStacked { get { return _isStacked; } }

    // To check if the item is in the burger stack
    bool _isStacked = false;

    // Identify the bottom bun to get the burger system
    BurgerStackSystem _burgerSystem = null;

    TrayStackSystem _traySystem = null;

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
        _isStacked = true;

        // Burger system is the burger system, so we need to store it in the item identifier to access the burger system later.
        _burgerSystem = burgerSystem;
    }

    public void AddItemToStack(TrayStackSystem traySystem)
    {
        _isStacked = true;
        _traySystem = traySystem;
    }

    public void RemoveItemFromStack()
    {
        _isStacked = false;
        _traySystem = null;
    }

    // Remove the burger item from the stack
    public void RemoveBurgerItemFromStack()
    {
        if(_burgerSystem != null)
        {
            // Nullify the burger system to avoid memory leak
            _burgerSystem = null;
            _isStacked = false;
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

public enum  TypeOfCardboardPackage
{
    Patty,
    Fries,
    Nuggets,
    Drink,
}
