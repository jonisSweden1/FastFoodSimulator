using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

// This script is in charge of handling the slots of item and updating UI in the inventory.
public class InventorySystem : MonoBehaviour
{
    // To-Do: Add new parameter such as the maximum number of slots.
    // This is to initialize the inventory system, and set the maximum number of slots in the inventory. 
    // This is going to be initialized together with the InventoryUI script, and the InventorySlot script, to make sure that the inventory system is working properly.
    [SerializeField]
    private int _maxSlots;

    private InventorySlot[] _inventorySlots;

    [SerializeField]
    private InventoryUI _inventoryUI;

    private int currentAvailableSlotIndex = 0;

    private void Start()
    {
        _inventorySlots = new InventorySlot[_maxSlots];
        for (int i = 0; i < _maxSlots; i++)
        {
            _inventorySlots[i] = new InventorySlot();
            Debug.Log(_inventorySlots[i]);
        }
        SortByName();
    }

    public void SortByName()
    {
        _inventorySlots = _inventorySlots
            .ToList()
            .OrderBy(slot => slot.IsEmpty)
            .ThenBy(slot => slot.ItemName)
            .ToArray();

        _inventoryUI.RefreshUI(_inventorySlots);
        _inventoryUI.SetAllButtonListener(this);
    }

    public int GetMaxSlots()
    {
        return _maxSlots;
    }

    // This method is to take an item from the inventory, or store it in the inventory slot.
    // This method is going to be called when the player pick a slot in the inventory, and the item is going to be taken out of the inventory, or stored in the inventory slot.
    // It is going to check if the GameObject item is null, and if it is not null, it is going to check if the item is already in the inventory, and if it is not, it is going to store it in the inventory slot.
    public bool TakeOrAddItemToTheInventory(int slotIndex, ref GameObject item, out InventorySlotState state)
    {
        if (item == null)
        {
            if(TryTakeOutItem(slotIndex, out item))
            {
                state = InventorySlotState.Empty;
                return true;
            }
            else
            {
                state = InventorySlotState.Empty;
                return false;
            }
        }
        else
        {
            if(TryAddStoredItemToInventory(item))
            {
                state = InventorySlotState.Occupied;
                return true;
            }
            else
            {
                state = InventorySlotState.Occupied;
                return false;
            }
        }
    }

    // This method is to add an item to the inventory, and store it in the inventory slot.
    public bool TryAddStoredItemToInventory(GameObject itemToStore)
    {
        if (itemToStore == null)
        {
            Debug.LogWarning("Item to store is null. Cannot add to inventory.");
            return false;
        }

        if(currentAvailableSlotIndex > _maxSlots)
        {
            Debug.LogWarning("Inventory is full. Cannot add more items.");
            return false;
            
        }

        if(!_inventorySlots[currentAvailableSlotIndex].TryStoreItem(itemToStore, itemToStore.name))
        {
            return false;
        }
        else
        {
            currentAvailableSlotIndex++;
            SortByName();
            return true;
        }
    }

    // This method is to remove an item from the inventory, and destroy it from the inventory slot.
    public void RemoveStoredItemFromInventory(int slotIndex)
    {
        _inventorySlots[slotIndex].RemoveItem();
        currentAvailableSlotIndex--;
        SortByName();
    }

    public bool TryTakeOutItem(int slotIndex, out GameObject itemToTakeOut)
    {
        itemToTakeOut = null;

        Debug.Log(slotIndex);

        if (_inventorySlots != null)
        {
            if(_inventorySlots[slotIndex].TryTakeItem(out itemToTakeOut))
            {
                currentAvailableSlotIndex--;
                SortByName();
                return true;
            }
        }

        return false;
    }
}

public enum InventorySlotState
{
    Empty,
    Occupied
}