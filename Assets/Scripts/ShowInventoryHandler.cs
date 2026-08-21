using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowInventoryHandler : MonoBehaviour
{
    private PlayerInput _playerInput;

    InputAction _showInventoryAction;

    bool _hasInventoryShowed = false;

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _showInventoryAction = _playerInput.actions.FindAction("Show Inventory");

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
            UserInterfaceManager.instance.GoToMenu(0); // Assuming 0 is the index of the inventory menu
            _hasInventoryShowed = true;
        }
        else
        {
            UserInterfaceManager.instance.CloseMenu();
            _hasInventoryShowed = false;
        }
    }
}
