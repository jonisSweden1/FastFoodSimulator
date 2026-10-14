using System.Collections.Generic;
using UnityEngine;

public class BurgerStackNoneState : BurgerStackBaseState
{
    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        return false;
    }

    public override void Enter(BurgerStackSystem manager, GameObject item)
    {
        
    }

    public override void Exit(BurgerStackSystem manager)
    {
        
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack)
    {
        // Setting the removedItem with the toppest stack burger item
        item = burgerStack[manager.CurrentStackIndex];

        // Remove parent from the stacked burger
        item.transform.SetParent(null, true);
        item.GetComponent<ItemIdentifier>().RemoveBurgerItemFromStack();

        // Removing the burger item from the stack
        burgerStack.RemoveAt(manager.CurrentStackIndex);

        return true;
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {
        
    }
}
