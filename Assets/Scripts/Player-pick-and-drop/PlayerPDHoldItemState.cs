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

        if(hit.collider.tag == "Obstacle")
        {
            return;
        }

        // TODO: Refactor this code to be more modular and easier to read,
        // possibly by creating strategy pattern for different item interactions,
        // or by creating separate methods for each interaction type.
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

            // Check if the raycasted item is a burger item
            if(raycastItemId.CurrentType == TypeOfItem.BurgerItem)
            {
                if(holdItemId.CurrentType == TypeOfItem.PattySpatula)
                {
                    Debug.Log("The raycasted item is a burger item and the held item is a patty spatula. Attempting to flip the patty.");
                    Patty pattyScript = raycastItem.GetComponent<Patty>();
                    if (pattyScript == null)
                    {
                        Debug.LogError("The raycasted burger item does not have a Patty script attached.");
                        return;
                    }
                    // Flip the patty
                    pattyScript.FlipPatty();
                    return;
                }
                else if(holdItemId.CurrentType == TypeOfItem.SauceDispenser)
                {
                    Debug.Log("The burger item is raycasted and the hold item is sauce dispenser. Preparing to sauce the dispenser");

                    SauceDispenser sauceDispenser;

                    if (!holdItem.TryGetComponent<SauceDispenser>(out sauceDispenser))
                    {
                        Debug.LogError("Cannot find sauce dispenser script");
                        return;
                    }

                    if (!sauceDispenser.SpraySauce(raycastItem.gameObject))
                    {
                        Debug.LogError("Couldn't spray sauce");
                        return;
                    }

                    return;
                }
                else if (holdItemId.CurrentType == TypeOfItem.BurgerItem)
                {
                    Debug.Log("Both the raycasted item and the held item are burger items. Attempting to add the held burger item to the raycasted burger system.");

                    // Check if the raycasted item has a BurgerSystem component
                    if (raycastItem.TryGetComponent<BurgerStackSystem>(out BurgerStackSystem raycastBurgerSystem))
                    {
                        Debug.Log("BurgerStackSystem component found on the raycasted item. Attempting to add the held burger item to the raycasted burger system.");

                        // Add the held burger item to the raycasted burger system
                        if (!raycastBurgerSystem.TryAddItemToBurger(holdItem.gameObject))
                        {
                            // Successfully added the held burger item to the raycasted burger system
                            Debug.Log("Failed to add " + holdItem.name + " to " + raycastItem.name);
                            return;
                        }

                        manager.ItemObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
                        manager.ItemObject = null;
                        manager.SwitchState(manager.nonItemState);

                        return;
                    }
                    else if (raycastItemId.BurgerSystem != null)
                    {
                        // Debug.Log("Raycasted item has a BurgerSystem reference. Attempting to add the held burger item to the raycasted burger system.");

                        // Add the held burger item to the raycasted burger system
                        if (!raycastItemId.BurgerSystem.TryAddItemToBurger(holdItem.gameObject))
                        {
                            // Successfully added the held burger item to the raycasted burger system
                            Debug.LogError("Failed to add " + holdItem.name + " to " + raycastItem.name);
                            return;
                        }

                        manager.ItemObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
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
            else if (raycastItemId.CurrentType == TypeOfItem.HeaterTray)
            {
                if (holdItemId.CurrentType == TypeOfItem.BurgerItem)
                {
                    Debug.Log("The raycasted item is a heater tray and the held item is a burger item. Attempting to add the held burger item to the heater tray.");
                    TrayStackSystem heaterTrayScript = raycastItem.GetComponent<TrayStackSystem>();
                    if (heaterTrayScript == null)
                    {
                        Debug.LogError("The raycasted heater tray does not have a TrayStackSystem script attached.");
                        return;
                    }
                    // Add the held burger item to the heater tray
                    if (!heaterTrayScript.TryAddToStack(holdItem.gameObject))
                    {
                        Debug.LogError("Failed to add " + holdItem.name + " to " + raycastItem.name);
                        return;
                    }
                    manager.ItemObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
                    manager.ItemObject = null;
                    manager.SwitchState(manager.nonItemState);
                    return;
                }
            }
        }

        // If no other items are identified anything different, then it is a item to drop
        manager.ItemObject.transform.parent = null;

        // Set item object's position and rotation to the point
        manager.ItemObject.transform.position = hit.point;
        manager.ItemObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

        // Debug.Log("Dropped item at: " + hit.point);

        // Remove item object from the pick-and-drop system
        manager.ItemObject.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        manager.ItemObject = null;

        // Switch state to non item state
        manager.SwitchState(manager.nonItemState);
    }
}