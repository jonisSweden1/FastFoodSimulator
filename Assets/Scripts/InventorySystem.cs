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

    // To interact between UI and the inventory system
    [SerializeField]
    private InventoryUI _inventoryUI;

    // To set the least available slot in the inventory
    private int currentAvailableSlotIndex = 0;

    private void Start()
    {
        // Debug.Log("Calling this method");

        // Initialize the inventory slots
        _inventorySlots = new InventorySlot[_maxSlots];
        for (int i = 0; i < _maxSlots; i++)
        {
            _inventorySlots[i] = new InventorySlot();
            //Debug.Log(_inventorySlots[i]);
        }

        // Sort items in the inventory
        SortByName();
    }

    public void SortByName()
    {
        // Sort items by which is empty first, then by name
        _inventorySlots = _inventorySlots
            .ToList()
            .OrderBy(slot => slot.IsEmpty)
            .ThenBy(slot => slot.ItemName)
            .ToArray();

        //Debug.Log(_inventorySlots.Count());

        // Refresh UI
        _inventoryUI.RefreshUI(_inventorySlots, this);
    }

    // This method is to take an item from the inventory, or store it in the inventory slot.
    // This method is going to be called when the player pick a slot in the inventory, and the item is going to be taken out of the inventory, or stored in the inventory slot.
    // It is going to check if the GameObject item is null, and if it is not null, it is going to check if the item is already in the inventory, and if it is not, it is going to store it in the inventory slot.
    public void TakeOrAddItemToTheInventory(int slotIndex)
    {
        // Identify which slot to take or add
        InventorySlot slot = _inventorySlots[slotIndex];

        GameObject item;

        if (slot != null)
        {
            if (slot.IsEmpty)
            // If a slot is empty, then store the item
            {
                // Debug.Log("Store item");

                // Try to take an item out from the player's hand
                if (PlayerPickAndDropSystem.Instance.TryTakeOutItem(out item))
                {
                    slot.StoreItem(item);
                    currentAvailableSlotIndex++;
                }
                // If there is no item in the player's hand, skip.
                else
                    return;
            }
            else
            // If a slot is full, then take the item
            {
                // Debug.Log("Take out item");

                // If the player is already holds an item, skip.
                // This is so that the next code line will not be executed
                if(PlayerPickAndDropSystem.Instance.HasObjectHold)
                {
                    return;
                }

                // Take an item out from the inventory on that specific slot
                item = slot.TakeItem();

                if (PlayerPickAndDropSystem.Instance.TryPickUpItem(item))
                {
                    currentAvailableSlotIndex--;
                }
                // If the player is already holds an item, skip.
                else
                    return;
            }

            // Sort items in the inventory
            SortByName();
        }
    }

    // This method is to remove an item from the inventory, and destroy it from the inventory slot.
    public void RemoveStoredItemFromInventory(int slotIndex)
    {
        _inventorySlots[slotIndex].RemoveItem();
        currentAvailableSlotIndex--;

        // Sort items in the inventory
        SortByName();
    }

    public int GetMaxSlots()
    {
        return _inventorySlots.Count();
    }
}

public enum InventorySlotState
{
    Empty,
    Occupied
}