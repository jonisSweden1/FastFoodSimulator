using System.Collections.Generic;
using UnityEngine;

public class StackingSystem : MonoBehaviour
{
    [SerializeField] private TypeOfBurgerStack allowedBurgerType; // The type of burger stack that this stacking system allows

    [SerializeField] private TypeOfStackStrategy stackStrategy;

    Dictionary<TypeOfStackStrategy, IStackingStrategy> _stackingStrategies;

    private int _currentStackIndex = 0;

    List<Transform> _stackedItems = new List<Transform>();

    private void Start()
    {
        _stackingStrategies = new Dictionary<TypeOfStackStrategy, IStackingStrategy>
        {
            { TypeOfStackStrategy.StackColumnOfRows, new RowColumnStackingStrategy() },
            { TypeOfStackStrategy.StackRow, new RowStackingStrategy() }
        };

        if (transform.childCount > 1)
        {
            Transform collectionPoint = transform.GetChild(1);
            foreach (Transform row in collectionPoint)
            {
                foreach (Transform pointItem in row)
                {
                    Debug.Log($"StackingSystem: Adding point item {pointItem.name} to stacked items.");
                    _stackedItems.Add(pointItem);
                }
            }
        }
    }

    public bool TryAddItemOnStack(GameObject item)
    {
        if (_stackingStrategies.TryGetValue(stackStrategy, out IStackingStrategy strategy))
        {
            return strategy.TryAddItemOnStack(item);
        }
        return false;
    }

    public bool TryRemoveFromStack(out GameObject burgerItem)
    {
        if (_stackingStrategies.TryGetValue(stackStrategy, out IStackingStrategy strategy))
        {
            return strategy.TryRemoveFromStack(out burgerItem);
        }
        burgerItem = null;
        return false;
    }
}       

public enum TypeOfStackStrategy
{
    StackColumnOfRows,
    StackRow,
}