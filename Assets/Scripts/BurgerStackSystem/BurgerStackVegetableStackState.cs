using System.Collections.Generic;
using UnityEngine;

public class BurgerStackVegetableStackState : BurgerStackBaseState
{
    public override void Enter(BurgerStackSystem manager, GameObject item)
    {
        if(manager.CurrentlyOnStacking)
        {
            manager.SpawnCucumberPicklePoints(manager.transform);
        }
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
