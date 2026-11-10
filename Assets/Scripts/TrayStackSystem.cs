using System.Collections.Generic;
using UnityEngine;

public class TrayStackSystem : MonoBehaviour
{
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
                    _stackedItems.Add(pointItem);
                }
            }
        }
    }

    public void AddPattyToStack(GameObject burgerItem)
    {
        // Get the ItemIdentifier component from the burger item
        ItemIdentifier itemIdentifier = burgerItem.GetComponent<ItemIdentifier>();
        if (itemIdentifier != null)
        {
            if (itemIdentifier.CurrentType == TypeOfItem.BurgerItem)
            {
                if (itemIdentifier.CurrentBurgerType == TypeOfBurgerStack.Patty)
                {
                    if(_currentStackIndex >= _stackedItems.Count)
                    {
                        Debug.LogError($"TrayStackSystem: No more space to stack items.");
                        return;
                    }

                    burgerItem.transform.position = _stackedItems[_currentStackIndex].position;

                    itemIdentifier.AddItemToStack();
                    _currentStackIndex++;
                }
                else
                {
                    Debug.LogError($"TrayStackSystem: ItemIdentifier is not of type Patty on {burgerItem.name}.");
                }
            }
            else
            {
                Debug.LogError($"TrayStackSystem: ItemIdentifier component not found on {burgerItem.name}.");
            }
        }
    }

    public void RemovePattyFromStack(GameObject burgerItem)
    {
        // Get the ItemIdentifier component from the burger item
        ItemIdentifier itemIdentifier = burgerItem.GetComponent<ItemIdentifier>();
        if (itemIdentifier != null)
        {
            if (itemIdentifier.CurrentType == TypeOfItem.BurgerItem)
            {
                if (itemIdentifier.CurrentBurgerType == TypeOfBurgerStack.Patty)
                {
                    if (_currentStackIndex <= 0)
                    {
                        Debug.LogError($"TrayStackSystem: No more items to remove from stack.");
                        return;
                    }
                    itemIdentifier.RemoveItemFromStack();
                    _currentStackIndex--;
                }
                else
                {
                    Debug.LogError($"TrayStackSystem: ItemIdentifier is not of type Patty on {burgerItem.name}.");
                }
            }
            else
            {
                Debug.LogError($"TrayStackSystem: ItemIdentifier component not found on {burgerItem.name}.");
            }
        }
    }
}
