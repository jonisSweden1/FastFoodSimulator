using UnityEngine;

public class PaperActivationBurgerStack : MonoBehaviour
{
    public void AddBottomBun(Transform bottomBunItem)
    {
        bottomBunItem.position = transform.position;
        bottomBunItem.parent = transform;

        BurgerSystem burgerSystem;

        if (bottomBunItem.TryGetComponent<BurgerSystem>(out burgerSystem))
        {
            burgerSystem.ActivateBurgerStack();
        }
    }
}
