using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private List<InventoryItem> _inventoryItems;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform inventoryParent = transform.GetChild(0);

        InventorySystem[] inventoryItems = inventoryParent.GetComponentsInChildren<InventorySystem>();

        _inventoryItems = new List<InventoryItem>();
        _inventoryItems.AddRange(inventoryItems.Select(item => item.GetComponent<InventoryItem>()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInventory(InventoryItem item)
    {
        _inventoryItems.Add(item);
    }

    public void RemoveItemFromInventory(InventoryItem item)
    {
        _inventoryItems.Remove(item);
    }

    public void TakeItemFromInventory(InventoryItem item)
    {
        if (_inventoryItems.Contains(item))
        {
            _inventoryItems.Remove(item);
            item.gameObject.SetActive(true);
        }
    }
}
