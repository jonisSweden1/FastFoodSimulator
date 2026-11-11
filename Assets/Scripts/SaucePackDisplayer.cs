using UnityEngine;

public class SaucePackDisplayer : MonoBehaviour
{
    private SaucePackage _saucePackage;

    [SerializeField] private Transform sauceObject;

    private void OnEnable()
    {
        Debug.Log("SaucePackDisplayer Start");

        if (!transform.parent.TryGetComponent<SaucePackage>(out _saucePackage))
        {
            Debug.LogError("Cannot find sauce package script");
            return;
        }

        Debug.Log(_saucePackage);

        if (_saucePackage != null)
        {
            Debug.Log("Subscribing to OnSauceAmountChanged event");
            _saucePackage.OnSauceAmountChanged += UpdateSauceDisplay;
        }
    }

    private void OnDisable()
    {
        if (_saucePackage != null)
        {
            Debug.Log("Unsubscribing from OnSauceAmountChanged event");
            _saucePackage.OnSauceAmountChanged -= UpdateSauceDisplay;
        }
    }

    private void UpdateSauceDisplay(float newAmountPercentage)
    {
        Debug.Log($"Updating sauce display to {newAmountPercentage * 100}%");

        if (sauceObject == null)
        {
            Debug.LogError("Cannot find sauce object");
            return;
        }

        // Update the y scale of the sauce object based on the new amount percentage
        Vector3 newScale = sauceObject.localScale;
        newScale.y = newAmountPercentage;
        sauceObject.localScale = newScale;

        // Update the visual display of the sauce amount
    }
}
