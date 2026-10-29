using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventorySlotPrefab;

    [SerializeField]
    private Transform _collectionObject;

    [SerializeField]
    private PlayerInput _playerInput;

    public static InventoryUI Instance { get; private set; }

    private void Start()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void OpenUI()
    {
        // Debug.Log("Show Inventory");

        // Showing the menu
        UserInterfaceManager.instance.GoToMenu(0); // Assuming 0 is the index of the inventory menu

        // Switching the current action map to UI
        _playerInput.SwitchCurrentActionMap("UI");

        InputAction quitInventoryAction = _playerInput.currentActionMap.FindAction("QuitInventory");
        quitInventoryAction.started += ((Callback) => CloseUI());

        // Show the mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseUI()
    {
        // Closing the current menu
        UserInterfaceManager.instance.CloseMenu();

        // Switching the current action map to Player
        _playerInput.SwitchCurrentActionMap("Player");

        // Hide the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RefreshUI(InventorySlot[] slots, InventorySystem inventorySystem)
    {
        slots = slots.ToList()
            .OrderBy(slot => slot.IsEmpty)
            .ThenBy(slot => slot.ItemName)
            .ToArray();

        // If the there is no object that acts like a collection, then skip.
        if (_collectionObject == null) return;

        // Destroy all objects that were there.
        for(int i = _collectionObject.childCount - 1; i >= 0; i--)
        {
            Destroy(_collectionObject.GetChild(i).gameObject);
        }

        // Add new slots according to the inventory system
        for (int i = 0; i < slots.Length; i++)
        {
            // Instantitate slot
            GameObject slot = Instantiate(_inventorySlotPrefab, _collectionObject);

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
