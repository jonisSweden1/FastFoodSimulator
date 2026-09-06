using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// This script is in charge of handling the slots of item and updating UI in the inventory.
public class InventorySystem : MonoBehaviour
{
    private List<InventorySlot> _inventorySlots;

    private int currentAvailableSlotIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform inventoryParent = transform.GetChild(0);

        InventorySlot[] inventorySlots = inventoryParent.GetComponentsInChildren<InventorySlot>();

        _inventorySlots = new List<InventorySlot>();
        _inventorySlots.AddRange(inventorySlots.Select(item => item.GetComponent<InventorySlot>()));

        foreach (InventorySlot item in inventorySlots)
        {
            // Later on, this will be integrated with the pick and drop system.
            // In this feature, the object that the player is holding will determine which item will be placed, and which item can take out.
            item.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeStoredItemFromInventory(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is to take an item from the inventory, or add an item to the inventory.
    // This method will be integrated with the pick and drop system, where the player can pick up an item and store it in the inventory, or take an item from the inventory and use it.
    public void TakeOrAddItemToTheInventory(InventorySlot itemSlot, GameObject itemToStore)
    {
        
    }

    public void AddStoredItemToInventory(GameObject itemToStore)
    {
        if (itemToStore != null)
        {
            if(currentAvailableSlotIndex < _inventorySlots.Count)
            {
                itemToStore.SetActive(false);
                _inventorySlots[currentAvailableSlotIndex].AddItem(itemToStore);
                currentAvailableSlotIndex++;
            }
        }
    }

    public void RemoveStoredItemFromInventory(InventorySlot itemSlot)
    {
        GameObject storedItem;

        if (_inventorySlots.Contains(itemSlot) && _inventorySlots.Count > 0 )
        {
            if(itemSlot.TryToTakeOut(out storedItem))
            {
                Destroy(storedItem);
                --currentAvailableSlotIndex;
            }
        }
    }

    // This is to take a stored item in the inventory, and use it for the player to pick up
    public void TakeStoredItemFromInventory(InventorySlot itemSlot)
    {
        Debug.Log("Method called");

        GameObject storedItem;

        if (_inventorySlots.Contains(itemSlot))
        {
            if(itemSlot.TryToTakeOut(out storedItem))
            {
                // Add logic to integrate with Pick and Drop system
            }
        }
    }
}
