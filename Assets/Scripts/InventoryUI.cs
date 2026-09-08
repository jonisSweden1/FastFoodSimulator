using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventorySlotPrefab;

    [SerializeField]
    private InventorySystem _inventorySystem;

    public void RefreshUI(InventorySlot[] slots)
    {
        int totalSlots = slots.Length;

        Transform collectionObject = transform.GetChild(0);

        // Clear existing slots
        if(collectionObject != null )
        {
            if(collectionObject.childCount > 0)
            {
                foreach (Transform child in collectionObject)
                {
                    Destroy(child.gameObject);
                }
            }

            // Create new slots based on the inventory system
            for (int i = 0; i < totalSlots; i++)
            {
                GameObject slot = Instantiate(_inventorySlotPrefab, collectionObject);
                // You can add additional logic here to set up the slot based on the inventory data

                slot.GetComponent<Button>().onClick.AddListener(() => { _inventorySystem.TryTakeOutItem(i, out GameObject item); });
            }
        }
    }
}
