using UnityEngine;

public class SauceDispenserDropItemStrategy : IDropItemStrategy
{
    public void DropItem(PlayerPickAndDropSystem manager, Transform raycastItem, Transform holdItem, ItemIdentifier raycastItemId, ItemIdentifier holdItemId)
    {
        if (holdItemId.CurrentType == TypeOfItem.SaucePackage)
        {
            Debug.Log("The raycasted item is a sauce dispenser and the held item is a sauce package. Preparing to fill the dispenser.");
            SaucePackage saucePackage;
            SauceDispenser sauceDispenser;

            if (!raycastItem.TryGetComponent<SauceDispenser>(out sauceDispenser))
            {
                Debug.LogError("Cannot find sauce dispenser script");
                return;
            }

            if (!holdItem.TryGetComponent<SaucePackage>(out saucePackage))
            {
                Debug.LogError("Cannot find sauce package script");
                return;
            }

            if (raycastItemId.CurrentSauceType != holdItemId.CurrentSauceType)
            {
                Debug.LogError("The sauce types of the dispenser and package do not match.");
                return;
            }

            float amountToFill = sauceDispenser.MaxAmountSauce - (sauceDispenser.GetCurrentAmountPercentage() * sauceDispenser.MaxAmountSauce);

            // Use the sauce from the sauce package
            float usedSauceAmount = saucePackage.UseSauce(amountToFill); // Use the required amount of sauce

            // Here you can implement logic to apply the used sauce amount to the burger item
            Debug.Log($"Applied {usedSauceAmount} units of sauce to {raycastItem.name}.");
        }
    }
}

public class BurgerItemDropItemStrategy : IDropItemStrategy
{
    public void DropItem(PlayerPickAndDropSystem manager, Transform raycastItem, Transform holdItem, ItemIdentifier raycastItemId, ItemIdentifier holdItemId)
    {
        if (holdItemId.CurrentType == TypeOfItem.BurgerItem)
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
}

public class HeaterTrayDropItemStrategy : IDropItemStrategy
{
    public void DropItem(PlayerPickAndDropSystem manager, Transform raycastItem, Transform holdItem, ItemIdentifier raycastItemId, ItemIdentifier holdItemId)
    {
        if (holdItemId.CurrentType == TypeOfItem.BurgerItem)
        {
            Debug.Log("The raycasted item is a heater tray and the held item is a burger item. Attempting to add the held burger item to the heater tray.");
            StackingSystem stackSystem = raycastItem.GetComponent<StackingSystem>();
            if (stackSystem == null)
            {
                Debug.LogError("The raycasted heater tray does not have a StackingSystem script attached.");
                return;
            }
            // Add the held burger item to the heater tray
            if (!stackSystem.TryAddItemOnStack(holdItem.gameObject))
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