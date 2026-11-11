using UnityEngine;

public class SaucePackage : MonoBehaviour
{
    [SerializeField] private float _sauceAmount = 100f; // The initial amount of sauce in the package

    private float _maxSauceAmount;

    public delegate void SauceAmountChanged(float newAmountPercentage);
    public event SauceAmountChanged OnSauceAmountChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _maxSauceAmount = _sauceAmount; // Store the initial amount as the maximum amount
    }

    public float UseSauce(float amount)
    {
        if (_sauceAmount >= amount)
        {
            _sauceAmount -= amount;
            Debug.Log($"Used {amount} sauce. Remaining sauce: {_sauceAmount}");
            OnSauceAmountChanged?.Invoke(_sauceAmount / _maxSauceAmount);
            return amount;
        }
        else
        {
            Debug.Log("Not enough sauce left in the package. Using the last amount.");
            float remainingSauce = _sauceAmount;
            _sauceAmount = 0;
            OnSauceAmountChanged?.Invoke(_sauceAmount / _maxSauceAmount);
            return remainingSauce;
        }
    }
}
