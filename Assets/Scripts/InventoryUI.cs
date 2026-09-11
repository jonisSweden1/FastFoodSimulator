using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventorySlotPrefab;

    [SerializeField]
    private Transform _collectionObject;

    [SerializeField]
    private InventorySystem _inventorySystem;

    public void RefreshUI(InventorySlot[] slots)
    {
        Transform collectionObject = transform.GetChild(0);

        int totalSlots = slots.Length;

        if(collectionObject != null)
        {
            if (collectionObject.childCount > 0)
            {
                foreach (Transform child in collectionObject)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < slots.Length; i++)
            {
                GameObject slot = Instantiate(_inventorySlotPrefab, collectionObject);

                slot.GetComponent<Image>().color = slots[i].IsEmpty ? Color.white : Color.red;
            }
        }
    }

    public void SetAllButtonListener(InventorySystem inventorySystem)
    {
        Transform collectionObject = transform.GetChild(0);

        for(int i = 0; i < collectionObject.childCount; i++)
        {
            int index = i;
            Button button = collectionObject.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => 
            {
                inventorySystem.TakeOrAddItemToTheInventory(index);
            });
        }
    }
}
