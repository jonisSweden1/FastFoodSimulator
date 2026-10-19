using UnityEngine;

public class SauceDispenser : MonoBehaviour
{
    [SerializeField] private GameObject _saucePrefab;

    [SerializeField] private TypeOfSauce _sauceType;

    private float currentAmountSauce = 0;

    // Max amount of sauce
    [SerializeField] private float maxAmountSauce = 60;

    // Amount of sauce that is going to be sprayed
    [SerializeField] private float spraySauceAmount = 5;

    public bool SpraySauce(GameObject targetObject)
    {
        if(currentAmountSauce <= 0)
        {
            Debug.LogError("Cannot spray anything");
            return false;
        }

        GameObject sauceInstance = Instantiate(_saucePrefab);

        if(!targetObject.GetComponent<ItemIdentifier>().BurgerSystem.TryAddItemToBurger(sauceInstance))
        {
            Debug.LogError("Cannot add sauce to the burger stack");
            Destroy(sauceInstance);

            return false;
        }

        return true;
    }

    public float GetCurrentAmountPercentage()
    {
        return currentAmountSauce / maxAmountSauce;
    }
}

public enum TypeOfSauce
{
    Ketchup,
    Mayonnaise,
    Mustard,
}