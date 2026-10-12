using UnityEngine;

public abstract class BurgerStackBaseState
{
    public abstract void Enter(BurgerStackSystem manager);
    public abstract void AddItem(BurgerStackSystem manager, GameObject item);
    public abstract void RemoveItem(BurgerStackSystem manager, out GameObject item);
    public abstract void Exit(BurgerStackSystem manager);
    public abstract void WrapPaper(BurgerStackSystem manager);
}
