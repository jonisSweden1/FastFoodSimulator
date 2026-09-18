using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Handler for opening the inventory UI
public class ShowInventoryHandler : MonoBehaviour
{
    private PlayerInput _playerInput;

    InputAction _showInventoryAction;

    bool _hasInventoryShowed = false;

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _showInventoryAction = _playerInput.actions.FindAction("Show Inventory");

        Debug.Log(_showInventoryAction);

        _showInventoryAction.started += _showInventoryAction_started;
    }

    private void OnDisable()
    {
        _showInventoryAction.started -= _showInventoryAction_started;
    }

    private void _showInventoryAction_started(InputAction.CallbackContext context)
    {
        if (!_hasInventoryShowed)
        {
            Debug.Log("Show Inventory");

            UserInterfaceManager.instance.GoToMenu(0); // Assuming 0 is the index of the inventory menu

            _playerInput.SwitchCurrentActionMap("UI");
            _showInventoryAction =_playerInput.currentActionMap.FindAction("Show Inventory");
            _showInventoryAction.started += _showInventoryAction_started;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _hasInventoryShowed = true;
        }
        else
        {
            Debug.Log("Not showing inventory");

            UserInterfaceManager.instance.CloseMenu();

            _playerInput.SwitchCurrentActionMap("Player");
            _showInventoryAction = _playerInput.currentActionMap.FindAction("Show Inventory");
            _showInventoryAction.started += _showInventoryAction_started;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _hasInventoryShowed = false;
        }
    }
}
