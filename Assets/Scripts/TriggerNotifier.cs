using UnityEngine;

public class TriggerNotifier : MonoBehaviour
{
    public delegate void TriggerEvent(Collider other);
    public event TriggerEvent OnTriggerCollided;
    public event TriggerEvent OnTriggerUncollided;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        if (_collider == null)
        {
            Debug.LogError("TriggerNotifier requires a Collider component.");
        }
    }

    private bool _isNotifying = false;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerCollided?.Invoke(other);
        _isNotifying = false; // Reset the flag when a new trigger is entered
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerUncollided?.Invoke(other);
        _isNotifying = true; // Set the flag to true when the trigger is exited
    }

    private void Update()
    {
        // Check if the collider is enabled
        if (GetComponent<Collider>() != null && !GetComponent<Collider>().enabled && !_isNotifying)
        {
            _isNotifying = true;
            OnTriggerUncollided?.Invoke(_collider); // Notify that the collider is disabled
            //Debug.LogWarning("Collider is disabled on " + gameObject.name);
        }
    }
}
