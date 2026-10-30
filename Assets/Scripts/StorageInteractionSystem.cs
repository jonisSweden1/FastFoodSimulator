using UnityEngine;

public class StorageInteractionSystem : MonoBehaviour
{
    public void InteractStorage()
    {
        PlayerHoverHandler.Instance.ResetHover();
        GetComponent<InventorySystem>().OpenInventory();
    }
}
