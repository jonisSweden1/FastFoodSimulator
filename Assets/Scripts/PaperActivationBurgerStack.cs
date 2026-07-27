using UnityEngine;

public class PaperActivationBurgerStack : MonoBehaviour
{
    [SerializeField] private GameObject bottomBun;

    public void AddBottomBun()
    {
        if (bottomBun != null)
        {
            GameObject instance = Instantiate(bottomBun, transform.position, Quaternion.identity, transform);

            BurgerSystem burgerSystemInInstance;

            if(instance.TryGetComponent<BurgerSystem>(out burgerSystemInInstance))
            {
                burgerSystemInInstance.ActivateBurgerStack();
            }
        }
    }
}
