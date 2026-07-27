using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    bool itHasInvoked = false;

    float time = 0;

    [SerializeField] private float maxTimeToActivate = 1;
    [SerializeField] private UnityEvent eventHandler;

    // Update is called once per frame
    void Update()
    {
        if (time < maxTimeToActivate)
            time += Time.deltaTime;
        else if (time > maxTimeToActivate)
        {
            if(eventHandler != null && !itHasInvoked)
            {
                eventHandler.Invoke();
                itHasInvoked = true;
            }
        }
    }
}
