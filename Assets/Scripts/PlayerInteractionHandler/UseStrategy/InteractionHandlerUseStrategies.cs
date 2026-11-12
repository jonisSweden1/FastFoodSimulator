using UnityEngine;

public class ItemUseStrategy : IInteractionHandlerUseStrategy
{
    public void Use(RaycastHit hit)
    {
        Transform item = hit.collider.transform.parent;
        ItemIdentifier itemId;

        // Check if the object is an item, which should have the ItemIdentifier
        if (item.TryGetComponent<ItemIdentifier>(out itemId))
        {
            if (itemId.CurrentType == TypeOfItem.WrappedPaper)
            {
                item.GetComponent<BurgerInfo>().UnwrapPaper();
            }

            // Check if the object is a burger item
            else if (itemId.CurrentType == TypeOfItem.BurgerItem)
            {
                // Check if it is stacked on the burger
                if (itemId.IsBurgerStacked)
                {
                    itemId.BurgerSystem.WrapPaper();
                }
            }
        }
    }
}

public class StorageUseStrategy : IInteractionHandlerUseStrategy
{
    public void Use(RaycastHit hit)
    {
        Debug.Log("Found Storage, accessing the inventory");

        hit.collider.GetComponent<StorageInteractionSystem>().InteractStorage();
    }
}
