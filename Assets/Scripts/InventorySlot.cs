using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class InventorySlot
{
    private GameObject _itemObject;

    public bool IsEmpty { get; private set; }
    public string ItemName { get; private set; }

    public InventorySlot()
    {
        IsEmpty = true;
        ItemName = string.Empty;
    }

    public void StoreItem(GameObject item)
    {
        _itemObject = item;
        IsEmpty = false;
        ItemName = item.name;
    }

    public bool TryTakeItem(out GameObject item)
    {
        item = null;
        if (_itemObject != null)
        {
            item = _itemObject;
            _itemObject = null;
            return true;
        }
        return false;
    }

    public void RemoveItem()
    {
        _itemObject = null;
        IsEmpty = true;
        ItemName = string.Empty;
    }
}
