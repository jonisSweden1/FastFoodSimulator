using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    // Reference to the GameObject representing the item in the slot
    private GameObject _itemInSlot;

    public void Start()
    {
        if(_prefab != null)
        {
            _itemInSlot = _prefab;
        }
    }

    // Can take out item from the slot
    public bool TryToTakeOut(out GameObject item)
    {
        if(_itemInSlot != null)
        {
            item = _itemInSlot;
            _itemInSlot = null;
            return true;
        }

        item = null;
        return false;
    }

    public bool CheckIfItemInSlot()
    {
        if (_itemInSlot != null)
            return true;

        return false;
    }

    // Can add item, but can't be replaced
    public void AddItem(GameObject item)
    {
        if(_itemInSlot == null)
        {
            _itemInSlot = item;
        }
        else
        {
            Debug.LogError("There is already an item in the slot");
        }
    }
}
