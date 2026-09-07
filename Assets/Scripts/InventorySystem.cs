using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

// This script is in charge of handling the slots of item and updating UI in the inventory.
public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    private Transform _inventoryParent;

    private List<InventorySlot> _inventorySlots;

    [SerializeField]
    private InventoryUI _inventoryUI;

    private int currentAvailableSlotIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventorySlot[] inventorySlots = _inventoryParent.GetComponentsInChildren<InventorySlot>();

        _inventorySlots = new List<InventorySlot>();
        _inventorySlots.AddRange(inventorySlots.Select(item => item.GetComponent<InventorySlot>()));

        Debug.Log("Inventory Slots Count: " + _inventorySlots.Count);

        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            InventorySlot item = _inventorySlots[i];

            item.Initialize();

            item.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeStoredItemFromInventory(item, out GameObject storedItem));
        }

        _inventorySlots = inventorySlots
            .OrderBy(slot => slot.CheckIfItemInSlot() ? 1 : 0)
            .ToList();

        if (_inventoryUI != null)
            _inventoryUI.RefreshUI();

        Debug.Log("Calling SortItemsInInventory() from Start() method");
    }

    private void OnCoroutineDone()
    {
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            int slotIndex = 0;

            InventorySlot item = _inventorySlots[i];
            if (item.CheckIfItemInSlot())
            {
                item.SetSlotIndex(slotIndex);
                slotIndex++;
                Debug.Log("Item in slot: " + item.name + " with index: " + i);
            }
        }
    }

    // This method is to take an item from the inventory, or add an item to the inventory.
    // This method will be integrated with the pick and drop system, where the player can pick up an item and store it in the inventory, or take an item from the inventory and use it.
    public void TakeOrAddItemToTheInventory(InventorySlot itemSlot, ref GameObject itemToStore)
    {
        
    }

    // This method is to add an item to the inventory, and store it in the inventory slot.
    public void AddStoredItemToInventory(GameObject itemToStore)
    {
        if (itemToStore != null)
        {
            if(currentAvailableSlotIndex < _inventorySlots.Count && currentAvailableSlotIndex >= 0)
            {
                itemToStore.SetActive(false);
                _inventorySlots[currentAvailableSlotIndex].AddItem(itemToStore);
                currentAvailableSlotIndex++;
            }
        }
    }

    // This method is to remove an item from the inventory, and destroy it from the inventory slot.
    public void RemoveStoredItemFromInventory(InventorySlot itemSlot)
    {
        GameObject storedItem;

        if (_inventorySlots.Contains(itemSlot) && _inventorySlots.Count > 0 && currentAvailableSlotIndex >= 0)
        {
            if(itemSlot.TryToTakeOut(out storedItem))
            {
                Destroy(storedItem);
                --currentAvailableSlotIndex;
            }
        }
    }

    // This is to take a stored item in the inventory, and use it for the player to pick up
    public void TakeStoredItemFromInventory(InventorySlot itemSlot, out GameObject storedItem)
    {
        Debug.Log("Method called");

        storedItem = null;

        if (_inventorySlots.Contains(itemSlot))
        {
            if(itemSlot.TryToTakeOut(out storedItem))
            {
                Debug.Log("Item taken out from inventory: " + storedItem.name);

                // Add logic to integrate with Pick and Drop system
                --currentAvailableSlotIndex;
            }
        }
    }
}