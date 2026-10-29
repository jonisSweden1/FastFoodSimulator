using UnityEngine;

public class StorageInteractionSystem : MonoBehaviour
{
    public void InteractStorage()
    {
        GetComponent<InventorySystem>().OpenInventory();
    }
}
