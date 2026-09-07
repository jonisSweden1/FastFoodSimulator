using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// This script is in charge of handling the slots of item and updating UI in the inventory.
public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    private Transform _inventoryParent;

    private List<InventorySlot> _inventorySlots;

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

            if(item.CheckIfItemInSlot())
            {
                currentAvailableSlotIndex++;
                item.SetSlotIndex(currentAvailableSlotIndex);
            }
        }


        Debug.Log("Calling SortItemsInInventory() from Start() method");
        StartCoroutine(SortItemsInInventory());

        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            InventorySlot item = _inventorySlots[i];
            if (item.CheckIfItemInSlot())
            {
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
                StartCoroutine(SortItemsInInventory());

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

                StartCoroutine(SortItemsInInventory());

                // Add logic to integrate with Pick and Drop system
                --currentAvailableSlotIndex;
            }
        }
    }

    // This method is to sort the items in the inventory, and update the UI accordingly.
    private IEnumerator SortItemsInInventory()
    {
        // This method will be used to sort the items in the inventory, and update the UI accordingly.
        // The sorting can be based on item type, item name, or any other criteria.

        // Selection Sort Algorithm to sort the items in the inventory based on the order of the item in the slots in the inventory.
        // The items will be sorted in ascending order based on the order of the item in the slots in the inventory.
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            int min = i;
            for (int j = i + 1; j < _inventorySlots.Count; j++)
            {
                if (_inventorySlots[j].SlotIndex < _inventorySlots[min].SlotIndex)
                {
                    min = j;
                }
            }

            if(i != min)
            {
                GameObject temp;
                GameObject temp2;

                if (_inventorySlots[i].TryToTakeOut(out temp) && _inventorySlots[min].TryToTakeOut(out temp2))
                {
                    _inventorySlots[i].AddItem(temp2);
                    _inventorySlots[min].AddItem(temp);
                }
            }

            yield return null;
        }
    }
}
