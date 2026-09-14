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
        Transform collectionObject = transform.GetChild(0);
        if (collectionObject == null) return;

        for(int i = collectionObject.childCount - 1; i >= 0; i--)
        {
            Destroy(collectionObject.GetChild(i).gameObject);
        }

        for (int i = 0; i < slots.Length; i++)
        {
            GameObject slot = Instantiate(_inventorySlotPrefab, collectionObject);

            if(slot.TryGetComponent<Image>(out var image))
            {
                image.color = slots[i].IsEmpty ? Color.white : Color.red;
            }

            if(slot.TryGetComponent<Button>(out var button))
            {
                int index = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => inventorySystem.TakeOrAddItemToTheInventory(index));
            }
        }
    }
}
