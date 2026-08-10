using UnityEngine;

public class ItemIdentifier : MonoBehaviour
{
    public TypeOfItem CurrentType { get { return currentType; } }
    public TypeOfBurgerStack CurrentBurgerType { get { return currentBurgerType; } }

    [SerializeField] private TypeOfItem currentType;
    [SerializeField] private TypeOfBurgerStack currentBurgerType;
}

public enum TypeOfItem
{
    None,
    Paper,
    BottomBun,
    TopBun,
    BurgerItem,
}
