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

    public List<InventorySlot> InventorySlots { get; private set; } = new List<InventorySlot>();

    [SerializeField]
    private InventoryUI _inventoryUI;

    private int currentAvailableSlotIndex = 0;

    public void SortByName()
    {
        InventorySlots = InventorySlots
            .OrderBy(slot => slot.IsEmpty)
            .ThenBy(slot => slot.ItemName)
            .ToList();

        _inventoryUI.RefreshUI();
    }

    // This method is to take an item from the inventory, or add an item to the inventory.
    // This method will be integrated with the pick and drop system, where the player can pick up an item and store it in the inventory, or take an item from the inventory and use it.
    public void TakeOrAddItemToTheInventory(InventorySlot itemSlot, ref GameObject itemToStore)
    {
        
    }

    // This method is to add an item to the inventory, and store it in the inventory slot.
    public void AddStoredItemToInventory(GameObject itemToStore)
    {
        
    }

    // This method is to remove an item from the inventory, and destroy it from the inventory slot.
    public void RemoveStoredItemFromInventory(InventorySlot itemSlot)
    {
        
    }
}