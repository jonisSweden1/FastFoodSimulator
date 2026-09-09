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
    }

    public int GetMaxSlots()
    {
        return _maxSlots;
    }

    // This method is to take an item from the inventory, or add an item to the inventory.
    // This method will be integrated with the pick and drop system, where the player can pick up an item and store it in the inventory, or take an item from the inventory and use it.
    public void TakeOrAddItemToTheInventory(int slotIndex, ref GameObject itemToStore)
    {
        if(itemToStore == null)
        {
            if(TryTakeOutItem(slotIndex, out GameObject itemToTakeOut))
            {
                itemToStore = itemToTakeOut;
            }
        }
        else
        {
            AddStoredItemToInventory(itemToStore);
            itemToStore = null;
        }
    }

    // This method is to add an item to the inventory, and store it in the inventory slot.
    public void AddStoredItemToInventory(GameObject itemToStore)
    {
        if (itemToStore == null)
        {
            Debug.LogWarning("Item to store is null. Cannot add to inventory.");
            return;
        }

        if(currentAvailableSlotIndex < _maxSlots)
        {
            _inventorySlots[currentAvailableSlotIndex].StoreItem(itemToStore, itemToStore.name);
            currentAvailableSlotIndex++;
            SortByName();
        }
        else
        {
            Debug.LogWarning("Inventory is full. Cannot add more items.");
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