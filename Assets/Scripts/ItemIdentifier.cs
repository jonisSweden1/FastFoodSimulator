using UnityEngine;

public class ItemIdentifier : MonoBehaviour
{
    [SerializeField] private TypeOfItem currentType;
    [SerializeField] private TypeOfBurgerStack currentBurgerType;
}

public enum TypeOfItem
{
    None,
    Paper,
    BottomBun,
    BurgerItem,
}
