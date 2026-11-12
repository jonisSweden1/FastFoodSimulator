using System.Collections.Generic;
using UnityEngine;

public class StackingSystem : MonoBehaviour
{
    [SerializeField] private TypeOfBurgerStack _allowedBurgerType; // The type of burger stack that this stacking system allows

    [SerializeField] private TypeOfStackStrategy _currentStackStrategy;

    Dictionary<TypeOfStackStrategy, IStackSetupStrategy> _stackingStrategies;

    private int _currentStackIndex = 0;

    List<Transform> _stackedItems = new List<Transform>();

    private void Start()
    {
        _stackingStrategies = new Dictionary<TypeOfStackStrategy, IStackSetupStrategy>
        {
            { TypeOfStackStrategy.StackColumnOfRows, new RowColumnStackSetupStrategy() },
            { TypeOfStackStrategy.StackRow, new RowStackSetupStrategy() }
        };

        _stackingStrategies[_currentStackStrategy].SetupStack(transform, _allowedBurgerType, this, _stackedItems);
    }

    public bool TryAddItemOnStack(GameObject item)
    {
        if (_currentStackIndex < _stackedItems.Count)
        {
            Transform stackPoint = _stackedItems[_currentStackIndex];
            item.transform.position = stackPoint.position;
            item.transform.rotation = stackPoint.rotation;
            item.transform.SetParent(stackPoint);
            _currentStackIndex++;
            return true;
        }
        else
        {
            Debug.LogWarning("Stack is full. Cannot add more items.");
            return false;
        }
    }

    public bool TryRemoveFromStack(out GameObject burgerItem)
    {
        if(_currentStackIndex > 0)
        {
            _currentStackIndex--;
            Transform stackPoint = _stackedItems[_currentStackIndex];
            burgerItem = stackPoint.GetChild(0).gameObject;
            burgerItem.transform.SetParent(null);
            return true;
        }
        else
        {
            Debug.LogWarning("Stack is empty. Cannot remove items.");
            burgerItem = null;
            return false;
        }
    }
}       

public enum TypeOfStackStrategy
{
    StackColumnOfRows,
    StackRow,
}