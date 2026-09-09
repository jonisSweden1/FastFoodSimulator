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
        int totalSlots = slots.Length;

        if(_collectionObject != null)
        {

            if (_collectionObject.childCount > 0)
            {
                foreach (Transform child in _collectionObject)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < slots.Length; i++)
            {
                GameObject slot = Instantiate(_inventorySlotPrefab, _collectionObject);

                slot.GetComponent<Image>().color = slots[i].IsEmpty ? Color.white : Color.red;

                int indexCopy = i; // Create a copy of the index to avoid closure issues in the lambda expression

                slot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _inventorySystem.TryTakeOutItem(indexCopy, out GameObject item);
                });

                
            }
        }
    }
}
