using UnityEngine;

public class PlayerPDHoldItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager, Transform headTransform)
    {
        RaycastHit hit;
        if (!Physics.Raycast(headTransform.position, headTransform.forward, out hit, manager.DropDistance, manager.LayerToDrop))
        {
            return;
        }

        if (hit.collider.tag == "Item")
        {
            // Get the parent transform of the hit collider (the item being raycasted)
            Transform raycastItem = hit.collider.transform.parent;
            Transform holdItem = manager.ItemObject.transform;

            //Debug.Log("Raycast hit item: " + raycastItem.name);
            //Debug.Log("Held item: " + holdItem.name);

            // Get the ItemIdentifier components of both the raycasted item and the held item
            ItemIdentifier raycastItemId = raycastItem.GetComponent<ItemIdentifier>();
            ItemIdentifier holdItemId = holdItem.GetComponent<ItemIdentifier>();

            // TODO: Reimplement the logic of activating the burger system when placing the wrapper paper and the burger system
            // Check if the raycasted item is a burger item
            if (raycastItemId.CurrentType == TypeOfItem.BurgerItem && holdItemId.CurrentType == TypeOfItem.BurgerItem)
            {
                Debug.Log("Both the raycasted item and the held item are burger items. Attempting to add the held burger item to the raycasted burger system.");

                // Check if the raycasted item has a BurgerSystem component
                if (raycastItem.TryGetComponent<BurgerSystem>(out BurgerSystem raycastBurgerSystem))
                {
                    Debug.Log("BurgerSystem component found on the raycasted item. Attempting to add the held burger item to the raycasted burger system.");

                    // Add the held burger item to the raycasted burger system
                    if (!raycastBurgerSystem.TryAddItemToBurger(holdItem))
                    {
                        // Successfully added the held burger item to the raycasted burger system
                        Debug.Log("Failed to add " + holdItem.name + " to " + raycastItem.name);
                        return;
                    }

                    manager.ItemObject.SetActive(true);
                    manager.ItemObject = null;
                    manager.SwitchState(manager.nonItemState);

                    return;
                }
                else if (raycastItemId.BurgerSystem != null)
                {
                    Debug.Log("Raycasted item has a BurgerSystem reference. Attempting to add the held burger item to the raycasted burger system.");

                    // Add the held burger item to the raycasted burger system
                    if (!raycastItemId.BurgerSystem.TryAddItemToBurger(holdItem))
                    {
                        // Successfully added the held burger item to the raycasted burger system
                        Debug.Log("Failed to add " + holdItem.name + " to " + raycastItem.name);
                        return;
                    }
                    manager.ItemObject.SetActive(true);
                    manager.ItemObject = null;
                    manager.SwitchState(manager.nonItemState);
                    return;
                }
                else
                {
                    Debug.Log("Raycasted item does not have a BurgerSystem component or reference.");
                    return;
                }
            }
        }

        // If no other items are identified anything different, then it is a item to drop

        // Set item object's position and rotation to the point
        manager.ItemObject.transform.position = hit.point;
        manager.ItemObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        manager.ItemObject.SetActive(true);

        // Debug.Log("Dropped item at: " + hit.point);

        // Remove item object from the pick-and-drop system
        manager.ItemObject = null;

        // Switch state to non item state
        manager.SwitchState(manager.nonItemState);
    }
}