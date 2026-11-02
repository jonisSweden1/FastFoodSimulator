using UnityEngine;

public class SaucePackDisplayer : MonoBehaviour
{
    private SaucePackage _saucePackage;

    [SerializeField] private Transform sauceObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!transform.parent.TryGetComponent<SaucePackage>(out _saucePackage))
        {
            Debug.LogError("Cannot find sauce package script");
            return;
        }
    }

    private void OnEnable()
    {
        if (_saucePackage != null)
        {
            _saucePackage.OnSauceAmountChanged += UpdateSauceDisplay;
        }
    }

    private void OnDisable()
    {
        if (_saucePackage != null)
        {
            _saucePackage.OnSauceAmountChanged -= UpdateSauceDisplay;
        }
    }

    private void UpdateSauceDisplay(float newAmountPercentage)
    {
        if(sauceObject == null)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
