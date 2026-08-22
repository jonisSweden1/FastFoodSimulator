using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    // Indicates whether the item is currently in the inventory
    public bool IsInInventory { get; private set; } = true;

    // Reference to the GameObject representing the item in the inventory
    private GameObject itemInInventory;

    private InputAction _takeItem;

    public void Start()
    {
        itemInInventory = _prefab;
    }

    public bool TryTakeItemOut(out GameObject item)
    {
        if(IsInInventory)
        {
            item = itemInInventory;
            itemInInventory = null;

            IsInInventory = false;
            return true;
        }

        item = null;
        return false;
    }
}
