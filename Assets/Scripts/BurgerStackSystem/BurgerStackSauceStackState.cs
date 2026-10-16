using System.Collections.Generic;
using UnityEngine;

public class BurgerStackSauceStackState : BurgerStackBaseState
{
    public override void Enter(BurgerStackSystem manager, List<GameObject> burgerStack)
    {

    }

    public override void Exit(BurgerStackSystem manager)
    {
        
    }

    public override bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack)
    {
        throw new System.NotImplementedException();
    }

    public override bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack, out bool stackRemoved)
    {
        throw new System.NotImplementedException();
    }

    public override void WrapPaper(BurgerStackSystem manager)
    {

    }
}
