using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    public int SlotIndex { get; private set; } = -1;

    // Reference to the GameObject representing the item in the slot
    private GameObject _itemInSlot;

    public void Initialize()
    {
        if (_prefab != null)
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

            Debug.Log(item);

            _itemInSlot = null;

            // Change the color of the button to white to indicate that the slot is empty
            GetComponent<Image>().color = Color.white;

            return true;
        }

        item = null;
        return false;
    }

    public void SetSlotIndex(int index)
    {
        SlotIndex = index;
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

            // Change the color of the button to red to indicate that the slot is occupied
            // Later on, this will be changed to the sprite of the item that is in the slot.
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            Debug.LogError("There is already an item in the slot");
        }
    }
}
