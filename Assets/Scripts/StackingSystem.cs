using System.Collections.Generic;
using UnityEngine;

public class StackingSystem : MonoBehaviour
{
    [SerializeField] private TypeOfBurgerStack allowedBurgerType; // The type of burger stack that this stacking system allows

    private int _currentStackIndex = 0;

    List<Transform> _stackedItems = new List<Transform>();

    private void Start()
    {
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
        // Get the ItemIdentifier component from the burger item
        ItemIdentifier itemIdentifier = item.GetComponent<ItemIdentifier>();
        if (itemIdentifier != null)
        {
            if (itemIdentifier.CurrentType == TypeOfItem.BurgerItem)
            {
                if (itemIdentifier.CurrentBurgerType == allowedBurgerType)
                {
                    if (_currentStackIndex >= _stackedItems.Count)
                    {
                        Debug.LogError($"TrayStackSystem: No more space to stack items.");
                        return false;
                    }

                    item.transform.position = _stackedItems[_currentStackIndex].position;
                    item.transform.rotation = Quaternion.identity;
                    item.transform.SetParent(_stackedItems[_currentStackIndex]);

                    itemIdentifier.AddItemToStack(this);
                    _currentStackIndex++;
                    return true;
                }
                else
                {
                    Debug.LogError($"TrayStackSystem: ItemIdentifier is not of type Patty on {item.name}.");
                }
            }
            else
            {
                Debug.LogError($"TrayStackSystem: ItemIdentifier component not found on {item.name}.");
            }
        }
        return false;
    }

    public bool TryRemoveFromStack(out GameObject burgerItem)
    {
        if (_currentStackIndex <= 0)
        {
            Debug.LogError($"TrayStackSystem: No items to remove from stack.");
            burgerItem = null;
            return false;
        }

        burgerItem = _stackedItems[_currentStackIndex - 1].GetChild(0).gameObject;
        _currentStackIndex--;
        return true;
    }
}