using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public void RefreshUI()
    {
        Transform collection = transform.GetChild(0);

        InventorySlot[] uiSlots = collection.GetComponentsInChildren<InventorySlot>();


    }
}
