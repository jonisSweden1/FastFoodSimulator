using System.Collections.Generic;
using UnityEngine;

public class BurgerStackWrapState : BurgerStackBaseState
{
    public override void Enter(BurgerStackSystem manager, List<GameObject> burgerStack)
    {

    }

    public override void Exit(BurgerStackSystem manager)
    {

    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        return false;
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack)
    {
        item = null;
        return false;
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {
        manager.SwitchToPreviousState();
    }
}
