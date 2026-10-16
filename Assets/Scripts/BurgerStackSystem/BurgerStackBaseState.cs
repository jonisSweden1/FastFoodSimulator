using System.Collections.Generic;
using UnityEngine;

public abstract class BurgerStackBaseState
{
    public abstract void Enter(BurgerStackSystem manager, List<GameObject> burgerStack);
    public abstract bool AddItemToBurger(BurgerStackSystem manager, GameObject item, List<GameObject> burgerStack);
    public abstract bool RemoveItemFromBurger(BurgerStackSystem manager, out GameObject item, List<GameObject> burgerStack, out bool stackRemoved);
    public abstract void Exit(BurgerStackSystem manager);
    public abstract void WrapPaper(BurgerStackSystem manager);
}
