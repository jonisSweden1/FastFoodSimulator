using UnityEngine;
using UnityEngine.Events;

public class TriggerBoxHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _triggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _triggerEvent?.Invoke();
        }
    }
}
