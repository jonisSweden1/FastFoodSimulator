using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private List<GameObject> _inventoryItems;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inventoryItems = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInventory(GameObject item)
    {
        _inventoryItems.Add(item);
    }

    public void RemoveItemFromInventory(GameObject item)
    {
        _inventoryItems.Remove(item);
    }

    public void TakeItemFromInventory(GameObject item)
    {
        if (_inventoryItems.Contains(item))
        {
            _inventoryItems.Remove(item);
            item.SetActive(true);
        }
    }
}
