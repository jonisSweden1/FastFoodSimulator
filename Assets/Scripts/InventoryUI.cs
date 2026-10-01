using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventorySlotPrefab;

    [SerializeField]
    private InventorySystem _inventorySystem;

    public void RefreshUI(InventorySlot[] slots, InventorySystem inventorySystem)
    {
        // Get the object that collects all slots
        Transform collectionObject = transform.GetChild(0);

        // If the there is no object that acts like a collection, then skip.
        if (collectionObject == null) return;

        // Destroy all objects that were there.
        for(int i = collectionObject.childCount - 1; i >= 0; i--)
        {
            Destroy(collectionObject.GetChild(i).gameObject);
        }

        // Add new slots according to the inventory system
        for (int i = 0; i < slots.Length; i++)
        {
            // Instantitate slot
            GameObject slot = Instantiate(_inventorySlotPrefab, collectionObject);

            // Initialize the button's color based on whether the slot is empty or not
            // If the slot is empty, the color of the button is white
            // If not, the color of the button is red
            if(slot.TryGetComponent<Image>(out var image))
            {
                image.color = slots[i].IsEmpty ? Color.white : Color.red;
            }

            // Initialize the button's listener to the inventory system method: TakeOrAddItemToTheInventory()
            if(slot.TryGetComponent<Button>(out var button))
            {
                int index = i;

                // Refresh all listeners from the button
                button.onClick.RemoveAllListeners();

                // Add the listener
                button.onClick.AddListener(() => inventorySystem.TakeOrAddItemToTheInventory(index));
            }
        }
    }
}
