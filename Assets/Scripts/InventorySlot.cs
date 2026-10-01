using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class InventorySlot
{
    private GameObject _itemObject;

    // To check if the item is empty
    public bool IsEmpty { get; private set; }

    // To sort all items in the inventory
    public string ItemName { get; private set; }

    // To increase security of the items
    public InventorySlot()
    {
        IsEmpty = true;
        ItemName = string.Empty;
    }

    // Add item directly to the slot
    public void StoreItem(GameObject item)
    {
        _itemObject = item;
        IsEmpty = false;
        ItemName = item.GetComponent<ItemIdentifier>().ItemName;
    }

    // Remove item from the slot and taking out the item
    public GameObject TakeItem()
    {
        GameObject item;

        // If the player tries to remove an item from the slot, but there is no item, then it indirectly skips.
        if (_itemObject != null)
        {
            item = _itemObject;
            _itemObject = null;

            ItemName = string.Empty;
            IsEmpty = true;

            return item;
        }

        return null;
    }

    // Destroy the item from the slot
    public void RemoveItem()
    {
        _itemObject = null;
        IsEmpty = true;
        ItemName = string.Empty;
    }
}
