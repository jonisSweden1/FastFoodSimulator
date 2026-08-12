using UnityEngine;

public class ItemIdentifier : MonoBehaviour
{
    public TypeOfItem CurrentType { get { return currentType; } }
    public TypeOfBurgerStack CurrentBurgerType { get { return currentBurgerType; } }

    public GameObject BottomBun { get { return _bottomBun; } }
    public bool IsBurgerStacked { get { return _isBurgerStacked; } }

    bool _isBurgerStacked = false;
    GameObject _bottomBun = null;

    [SerializeField] private TypeOfItem currentType;
    [SerializeField] private TypeOfBurgerStack currentBurgerType;

    public void HasAddedBurgerItemToStack(GameObject bottomBun)
    {
        _isBurgerStacked |= (bottomBun != null);
        _bottomBun = bottomBun;
    }
}

public enum TypeOfItem
{
    None,
    BurgerItem,
}
