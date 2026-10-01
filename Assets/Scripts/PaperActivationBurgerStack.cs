using UnityEngine;

// TODO: Remove this class
// Instead that the paper activator activates the burger system, the burger system will be in the paper wrapper
public class PaperActivationBurgerStack : MonoBehaviour
{
    // Identify the bottom bun item and activate the burger system
    Transform _bottomBunItem = null;

    // Add the bottom bun to activate the burger system
    public void AddBottomBun(Transform bottomBunItem)
    {
        // Identify the burger system in the bottom bun
        BurgerSystem burgerSystem;

        // If there is no component as Burger System, then skip
        if (!bottomBunItem.TryGetComponent<BurgerSystem>(out burgerSystem))
        {
            return;
        }

        // Position and put the bottom bun to the parent
        bottomBunItem.position = transform.position;
        bottomBunItem.parent = transform;

        // Activate the burger system
        burgerSystem.ActivateBurgerStack();

        // Assign the bottom bun item to this script
        _bottomBunItem = bottomBunItem;
    }

    // Remove the bottom bun and deactivate the burger system
    public void RemoveBottomBun()
    {
        if (_bottomBunItem != null)
        {
            BurgerSystem burgerSystem;

            if (!_bottomBunItem.TryGetComponent<BurgerSystem>(out burgerSystem))
            {
                return;
            }

            // Remove the bottom bun from the paper parent
            _bottomBunItem.SetParent(null, true);

            // Deactivate the burger system
            burgerSystem.DeactivateBurgerStack();

            // Remove the bottom bun item from this script
            _bottomBunItem = null;
        }
    }
}
