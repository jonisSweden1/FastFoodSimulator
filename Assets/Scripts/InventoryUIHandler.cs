using UnityEngine;

public class InventoryUIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void UpdateUI()
    {
        GameObject[] inventoryItems = transform.GetChild(0).GetComponentsInChildren<GameObject>();

    }
}
