using System.Collections.Generic;
using UnityEngine;

public class BurgerStackWrapState : BurgerStackBaseState
{
    public override void Enter(BurgerStackSystem manager, GameObject item)
    {

    }

    public override void Exit(BurgerStackSystem manager)
    {

    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        throw new System.NotImplementedException();
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack)
    {
        throw new System.NotImplementedException();
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {

    }
}
