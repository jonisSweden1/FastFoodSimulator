using UnityEngine;

public class PlayerPDNonItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager, Transform headTransform)
    {
        // Perform a raycast to check if the player is looking at an item
        RaycastHit hit;
        if(!Physics.Raycast(headTransform.position, headTransform.forward, out hit, manager.PickDistance))
        {
            Debug.LogError($"Raycast could hit anything.");
            return;
        }
        
        // Check if the hit object has a collider
        if(hit.collider.gameObject == null)
        {
            Debug.LogError($"Object couldn't be found");
            return;
        }

        // Check if the hit object has the "Item" tag
        if (hit.collider.tag == "Item")
        {
            // Get the parent object of the hit collider, and get its ItemIdentifier component
            GameObject raycastItemObject = hit.collider.transform.parent.gameObject;
            ItemIdentifier raycastItemId = raycastItemObject.GetComponent<ItemIdentifier>();

            Debug.Log(raycastItemObject.name);

            // Check if the item is a burger item and is stacked
            if (raycastItemId.CurrentType == TypeOfItem.BurgerItem && raycastItemId.IsBurgerStacked)
            {
                // If the bottom bun of the burger stack is null, log an error and return
                // The information about the bottom bun is stored in the ItemIdentifier component of the burger item in order to access the burger stack system and remove the item from the stack.
                // If the bottom bun is null, it means that the burger stack is not properly set up, and we cannot remove the item from the stack.
                if (raycastItemId.BottomBun == null)
                {
                    Debug.LogError($"Bottom bun of the burger stack is null.");
                    return;
                }

                // Try to remove the item from the burger stack using the BurgerSystem component of the bottom bun
                // Otherwise, it will jump straight to the next if statement.
                if (raycastItemId.BottomBun.GetComponent<BurgerSystem>().RemoveItemFromBurger(out GameObject removedItem))
                {
                    removedItem.GetComponent<ItemIdentifier>().RemoveBurgerItemFromStack();
                    manager.ItemObject = removedItem;
                    manager.ItemObject.SetActive(false);
                    manager.SwitchState(manager.holdItemState);
                    return;
                }
            }

            // If the item that is raycasted is a bottom bun, we need to deactivate the burger stack system of the item, so that it can be picked up and held by the player.
            if (raycastItemId.CurrentBurgerType == TypeOfBurgerStack.BottomBun && raycastItemObject.transform.parent != null)
            {
                raycastItemObject.transform.parent.GetComponent<PaperActivationBurgerStack>().RemoveBottomBun();
            }

            manager.ItemObject = raycastItemObject;
            manager.ItemObject.SetActive(false);
            manager.SwitchState(manager.holdItemState);
        }
    }
}