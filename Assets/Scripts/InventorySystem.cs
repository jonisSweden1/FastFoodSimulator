using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// This script is in charge of handling the slots of item and updating UI in the inventory.
public class InventorySystem : MonoBehaviour
{
    private List<InventorySlot> _inventorySlots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform inventoryParent = transform.GetChild(0);

        InventorySlot[] inventorySlots = inventoryParent.GetComponentsInChildren<InventorySlot>();

        _inventorySlots = new List<InventorySlot>();
        _inventorySlots.AddRange(inventorySlots.Select(item => item.GetComponent<InventorySlot>()));

        foreach (InventorySlot item in inventorySlots)
        {
            item.gameObject.GetComponent<Button>().onClick.AddListener(() => TakeStoredItemFromInventory(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddStoredItemToInventory(InventorySlot itemSlot, GameObject itemToStore)
    {
        if (itemToStore != null)
        {
            if (_inventorySlots.Contains(itemSlot))
            {

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
            }
        }
    }

    // This is to take a stored item in the inventory, and use it for the player to pick up
    public void TakeStoredItemFromInventory(InventorySlot itemSlot)
    {
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
