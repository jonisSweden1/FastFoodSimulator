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
        ItemName = item.GetComponent<ItemIdentifier>().ItemName;
    }

    public GameObject TakeItem()
    {
        GameObject item;

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

    public void RemoveItem()
    {
        _itemObject = null;
        IsEmpty = true;
        ItemName = string.Empty;
    }
}
