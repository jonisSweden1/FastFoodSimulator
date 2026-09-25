using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Handler for opening the inventory UI
public class ShowInventoryHandler : MonoBehaviour
{
    // Property for player input
    private PlayerInput _playerInput;

    // Input action for showing the inventory UI
    InputAction _showInventoryAction;

    // Boolean for toggling the inventory UI
    bool _hasInventoryShowed = false;

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();

        // Find the action of show inventory
        _showInventoryAction = _playerInput.actions.FindAction("Show Inventory");

        // Debug.Log(_showInventoryAction);

        // Assign the event to open and close the inventory
        _showInventoryAction.started += _showInventoryAction_started;
    }

    private void OnDisable()
    {
        // Deassign the event to open and close the inventory
        _showInventoryAction.started -= _showInventoryAction_started;
    }

    private void _showInventoryAction_started(InputAction.CallbackContext context)
    {
        if (!_hasInventoryShowed)
        {
            // Debug.Log("Show Inventory");

            // Showing the menu
            UserInterfaceManager.instance.GoToMenu(0); // Assuming 0 is the index of the inventory menu

            // Switching the current action map to UI
            _playerInput.SwitchCurrentActionMap("UI");

            // Find and reassign the event to the method of opening and closing the inventory
            _showInventoryAction =_playerInput.currentActionMap.FindAction("Show Inventory");
            _showInventoryAction.started += _showInventoryAction_started;

            // Show the mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Set inventory visibility to true
            _hasInventoryShowed = true;
        }
        else
        {
            // Debug.Log("Not showing inventory");

            // Closing the current menu
            UserInterfaceManager.instance.CloseMenu();

            // Switching the current action map to Player
            _playerInput.SwitchCurrentActionMap("Player");

            // Find and reassign the event to the method of opening and closing the inventory
            _showInventoryAction = _playerInput.currentActionMap.FindAction("Show Inventory");
            _showInventoryAction.started += _showInventoryAction_started;

            // Hide the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Set inventory visibility to false
            _hasInventoryShowed = false;
        }
    }
}
