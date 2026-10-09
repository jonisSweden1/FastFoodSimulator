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

            GameObject itemToTakeOut;

            // Check if the item is a burger item and is stacked
            if (raycastItemId.CurrentType == TypeOfItem.BurgerItem && raycastItemId.IsBurgerStacked)
            {
                Debug.Log("Raycasted item is a burger item and is stacked. Attempting to remove it from the stack.");
                if(raycastItemId.BurgerSystem.TryRemoveItemFromBurger(out itemToTakeOut))
                {
                    manager.ItemObject = itemToTakeOut;
                    manager.ItemObject.SetActive(false);

                    manager.SwitchState(manager.holdItemState);
                    return;
                }
                else
                {
                    Debug.LogError($"Item is a burger item but couldn't be removed from the stack.");
                    return;
                }
            }

            Transform parentObject = raycastItemObject.transform.parent;

            Debug.Log(parentObject);

            manager.ItemObject = raycastItemObject;
            manager.ItemObject.SetActive(false);

            manager.SwitchState(manager.holdItemState);
        }
    }
}