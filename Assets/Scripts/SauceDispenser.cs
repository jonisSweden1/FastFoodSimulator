using UnityEngine;

// TODO: test it just as soon as the pick and drop system has been modified
public class SauceDispenser : MonoBehaviour
{
    [SerializeField] private GameObject _saucePrefab;

    [SerializeField] private TypeOfSauce _sauceType;

    private Animator _leverAnimator;

    private float _currentAmountSauce = 60;

    // Max amount of sauce
    [SerializeField] private float _maxAmountSauce = 60;

    // Amount of sauce that is going to be sprayed
    [SerializeField] private float _spraySauceAmount = 5;

    [SerializeField] private string _triggerName;

    private void Awake()
    {
        _leverAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    public bool SpraySauce(GameObject targetObject)
    {
        if(_currentAmountSauce <= 0)
        {
            Debug.LogError("Cannot spray anything");
            return false;
        }

        if(_leverAnimator == null)
        {
            Debug.LogError("There is no animator");
            return false;
        }

        GameObject sauceInstance = Instantiate(_saucePrefab);

        ItemIdentifier targerObId;

        if(!targetObject.TryGetComponent<ItemIdentifier>(out targerObId))
        {
            Debug.LogError("Cannot find item identifier");
            Destroy(sauceInstance);

            return false;
        }

        if(!targerObId.IsBurgerStacked)
        {
            Debug.LogError("The burger item is not stacked");
            Destroy(sauceInstance);

            return false;
        }

        if(!targerObId.BurgerSystem.TryAddItemToBurger(sauceInstance))
        {
            Debug.LogError("Cannot add sauce to the burger stack");
            Destroy(sauceInstance);

            return false;
        }

        _leverAnimator.Play("Spray");

        _currentAmountSauce -= _spraySauceAmount;

        return true;
    }

    public float GetCurrentAmountPercentage()
    {
        return _currentAmountSauce / _maxAmountSauce;
    }

    // TODO: integrate this with sauce storage
    public void FillDispenser(float amount)
    {
        if(amount > _maxAmountSauce)
        {
            _currentAmountSauce = _maxAmountSauce;
        }
        else if(amount < 0)
        {
            Debug.Log("Cannot fill with minus");
        }
        else
        {
            _currentAmountSauce = amount;
        }
    }
}

public enum TypeOfSauce
{
    Ketchup,
    Mayonnaise,
    Mustard,
}