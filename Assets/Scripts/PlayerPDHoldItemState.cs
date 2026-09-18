using UnityEngine;

public class PlayerPDHoldItemState : PlayerPDBaseState
{
    public override void EnterButton(PlayerPickAndDropSystem manager, Transform headTransform)
    {
        RaycastHit hit;
        if (!Physics.Raycast(headTransform.position, headTransform.forward, out hit, manager.DropDistance))
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
            if (raycastItemId.CurrentType == TypeOfItem.BurgerItem)
            {
                // Logic for adding bottom bun to paper in order to create a burger stack
                // The paper activate the burger stack system on the bottom bun
                if (raycastItemId.CurrentBurgerType == TypeOfBurgerStack.Paper)
                {
                    if (holdItemId.CurrentBurgerType == TypeOfBurgerStack.BottomBun)
                    {
                        raycastItem.GetComponent<PaperActivationBurgerStack>().AddBottomBun(holdItem);

                        holdItemId.AddBurgerItemToStack(holdItem.gameObject);

                        manager.ItemObject.SetActive(true);
                        manager.ItemObject = null;

                        manager.SwitchState(manager.nonItemState);
                        return;
                    }
                    else
                        return;
                }

                // TODO: Reimplement the logic of activating the burger system when placing the wrapper paper and the burger system
                // Logic for adding burger items to the stack
                // The burger stack is deactivated when the top bun is added to the stack
                else if (raycastItemId.IsBurgerStacked)
                {
                    if (holdItemId.CurrentBurgerType != TypeOfBurgerStack.Paper)
                    {
                        GameObject bottomBun = raycastItemId.BottomBun;

                        if(!bottomBun.GetComponent<BurgerSystem>().AddItemToBurger(holdItem))
                            return;
                        
                        holdItemId.AddBurgerItemToStack(bottomBun);

                        manager.ItemObject.SetActive(true);
                        manager.ItemObject = null;

                        manager.SwitchState(manager.nonItemState);
                        return;
                    }
                    else
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