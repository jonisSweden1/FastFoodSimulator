using UnityEngine;

public abstract class BurgerStackBaseState
{
    public abstract void Enter(BurgerSystem manager);
    public abstract void AddItem(BurgerSystem manager, GameObject item);
    public abstract void RemoveItem(BurgerSystem manager, out GameObject item);
    public abstract void Exit(BurgerSystem manager);
}
