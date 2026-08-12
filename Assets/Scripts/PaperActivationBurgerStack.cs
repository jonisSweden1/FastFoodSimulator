using UnityEngine;

public class PaperActivationBurgerStack : MonoBehaviour
{
    Transform _bottomBunItem = null;

    public void AddBottomBun(Transform bottomBunItem)
    {
        bottomBunItem.position = transform.position;
        bottomBunItem.parent = transform;

        BurgerSystem burgerSystem;

        if (!bottomBunItem.TryGetComponent<BurgerSystem>(out burgerSystem))
        {
            return;
        }

        burgerSystem.ActivateBurgerStack();

        _bottomBunItem = bottomBunItem;
    }

    public void RemoveBottomBun()
    {
        if (_bottomBunItem != null)
        {
            BurgerSystem burgerSystem;

            if(!_bottomBunItem.TryGetComponent<BurgerSystem>(out burgerSystem))
            {
                return;
            }

            burgerSystem.DeactivateBurgerStack();

            _bottomBunItem = null;
        }
    }
}
