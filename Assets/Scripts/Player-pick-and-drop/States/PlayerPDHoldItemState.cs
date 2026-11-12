using System.Collections.Generic;
using UnityEngine;

public class PlayerPDHoldItemState : PlayerPDBaseState
{
    private Dictionary<TypeOfItem, IDropItemStrategy> _dropItemStrategies;

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

            IDropItemStrategy dropItemStrategy;

            if(_dropItemStrategies.TryGetValue(raycastItemId.CurrentType, out dropItemStrategy))
            {
                dropItemStrategy.DropItem(manager, raycastItem, holdItem, raycastItemId, holdItemId);
                return;
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

    public override void InitializeState(PlayerPickAndDropSystem manager)
    {
        _dropItemStrategies = new Dictionary<TypeOfItem, IDropItemStrategy>
        {
            {TypeOfItem.SauceDispenser, new SauceDispenserDropItemStrategy() },
            {TypeOfItem.BurgerItem, new BurgerItemDropItemStrategy() },
            {TypeOfItem.HeaterTray, new HeaterTrayDropItemStrategy() }
        };
    }
}