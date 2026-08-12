using UnityEngine;

public class ItemIdentifier : MonoBehaviour
{
    // This is a data class that will be used to identify the type of item and the type of burger stack it belongs to.
    public TypeOfItem CurrentType { get { return currentType; } }
    public TypeOfBurgerStack CurrentBurgerType { get { return currentBurgerType; } }

    public GameObject BottomBun { get { return _bottomBun; } }
    public bool IsBurgerStacked { get { return _isBurgerStacked; } }

    bool _isBurgerStacked = false;
    GameObject _bottomBun = null;

    [SerializeField] private TypeOfItem currentType;
    [SerializeField] private TypeOfBurgerStack currentBurgerType;

    public void AddBurgerItemToStack(GameObject bottomBun)
    {
        _isBurgerStacked = true;
        _bottomBun = bottomBun;
    }

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
